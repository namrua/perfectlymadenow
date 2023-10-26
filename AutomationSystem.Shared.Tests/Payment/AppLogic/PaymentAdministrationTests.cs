using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Core.Payment.AppLogic;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Tests.TestingHelpers.Services.Identities.Extensions;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using Xunit;

namespace AutomationSystem.Shared.Tests.Payment.AppLogic
{
    public class PaymentAdministrationTests
    {
        private readonly Mock<IPaymentDatabaseLayer> paymentDbMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IEventDispatcher> eventDispatcherMock;
        private readonly Mock<IPayPalKeyConvertor> convertorMock;

        public PaymentAdministrationTests()
        {
            paymentDbMock = new Mock<IPaymentDatabaseLayer>();
            identityResolverMock = new Mock<IIdentityResolver>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
            convertorMock = new Mock<IPayPalKeyConvertor>();
        }

        #region GetPayPalKeyListItems() tests
        [Fact]
        public void GetPayPalKeyListItems_FilterPassToGetPayPalKeys_ReturnsPayPalKeys()
        {
            // arrange
            var filter = new PayPalKeyFilter();
            paymentDbMock.Setup(e => e.GetPayPalKeys(It.IsAny<PayPalKeyFilter>())).Returns(new List<PayPalKey>());
            var admin = CreateAdmin();

            // act
            admin.GetPayPalKeyListItems(filter);

            // assert
            paymentDbMock.Verify(e => e.GetPayPalKeys(filter), Times.Once);
        }

        [Fact]
        public void GetPayPalKeyListItems_PayPalKey_IsMappedToPayPalKeyListItem()
        {
            // arrange
            var listItem = new PayPalKeyListItem();
            var payPalKeys = new List<PayPalKey>
            {
                new PayPalKey()
            };
            paymentDbMock.Setup(e => e.GetPayPalKeys(It.IsAny<PayPalKeyFilter>())).Returns(payPalKeys);
            convertorMock.Setup(e => e.ConvertToPayPalKeyListDetail(It.IsAny<PayPalKey>())).Returns(listItem);
            var admin = CreateAdmin();

            // act
            var result = admin.GetPayPalKeyListItems();

            // assert
            Assert.Collection(result,
                item => Assert.Same(listItem, item));
        }

        #endregion

        #region GetNewPayPalKeyForEdit() tests
        [Fact]
        public void GetNewPayPalKeyForEdit_NoAccessToPayPalKey_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            identityResolverMock.Setup(e => e.CheckEntitleForUserGroup(
                It.IsAny<Entitle>(),
                It.IsAny<UserGroupTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<EntityTypeEnum?>(),
                It.IsAny<long?>(),
                It.IsAny<string>())).Throws(new EntitleAccessDeniedException("", Entitle.CorePayPalKeys));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetNewPayPalKeyForEdit(UserGroupTypeEnum.MainProfile, 1));
            identityResolverMock.Verify(e => e.CheckEntitleForUserGroup(
                Entitle.CorePayPalKeys,
                UserGroupTypeEnum.MainProfile,
                1,
                null,
                null,
                null), Times.Once);
        }

        [Fact]
        public void GetNewPayPalKeyForEdit_UserGroupTypeEnumAndUserGroupId_ReturnsPayPalKeyForEditWithDefValues()
        {
            // arrange
            var forEdit = new PayPalKeyForEdit();
            convertorMock.Setup(e => e.InitializePayPalKeyForEdit()).Returns(forEdit);
            var admin = CreateAdmin();

            // act
            var result = admin.GetNewPayPalKeyForEdit(UserGroupTypeEnum.MainProfile, 20);

            // assert
            Assert.Equal(UserGroupTypeEnum.MainProfile, result.Form.UserGroupTypeId);
            Assert.Equal(20, result.Form.UserGroupId);
            Assert.Equal("production", result.Form.Environment);
            Assert.True(result.Form.Active);
            Assert.Equal(LocalisationInfo.DefaultCurrency, result.Form.CurrencyId);
        }
        #endregion

        #region GetPayPalKeyForEditById() tests
        [Fact]
        public void GetPayPalKeyForEditById_PayPalKeyIsNull_ThrowsArgumentException()
        {
            // arrange
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns((PayPalKey)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetPayPalKeyForEditById(1));
        }

        [Fact]
        public void GetPayPalKeyForEditById_NoAccessToPayPalKey_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var payPalKey = new PayPalKey();
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(payPalKey);
            identityResolverMock.SetupCheckEntitleForPayPalKey().Throws(new EntitleAccessDeniedException("", Entitle.CorePayPalKeys));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetPayPalKeyForEditById(1));
            identityResolverMock.VerifyCheckEntitleForPayPalKey(Entitle.CorePayPalKeys, payPalKey, Times.Once());
        }

        [Fact]
        public void GetPayPalKeyForEditById_ForPayPalKeyInDb_ReturnsPayPalKeyForEditAndEventIsRaised()
        {
            // arrange
            var payPalKey = new PayPalKey();
            var form = new PayPalKeyForm();
            var forEdit = new PayPalKeyForEdit();
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(payPalKey);
            convertorMock.Setup(e => e.InitializePayPalKeyForEdit()).Returns(forEdit);
            convertorMock.Setup(e => e.ConvertoToPayPalKeyForm(It.IsAny<PayPalKey>())).Returns(form);
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PayPalAccountDeletingEvent>())).Returns(true);
            var admin = CreateAdmin();

            // act
            var result = admin.GetPayPalKeyForEditById(1);

            // assert
            Assert.Same(form, result.Form);
            Assert.Same(forEdit, result);
            Assert.True(result.CanDelete);
            convertorMock.Verify(e => e.InitializePayPalKeyForEdit(), Times.Once);
            convertorMock.Verify(e => e.ConvertoToPayPalKeyForm(payPalKey), Times.Once);
            eventDispatcherMock.Verify(e => e.Check(It.Is<PayPalAccountDeletingEvent>(x => x.PayPalKeyId == 1)), Times.Once);

        }
        #endregion

        #region GetPayPalKeyForEditByForm() tests
        [Fact]
        public void GetPayPalKeyForEditByForm_PayPalKeyForm_ReturnsPayPalKeyForEditAndEventIsRaised()
        {
            // arrange
            var form = new PayPalKeyForm
            {
                PayPalKeyId = 1
            };
            var forEdit = new PayPalKeyForEdit();
            convertorMock.Setup(e => e.InitializePayPalKeyForEdit()).Returns(forEdit);
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PayPalAccountDeletingEvent>())).Returns(true);
            var admin = CreateAdmin();

            // act
            var result = admin.GetPayPalKeyForEditByForm(form);

            // assert
            Assert.Same(form, result.Form);
            Assert.Same(forEdit, result);
            Assert.True(result.CanDelete);
            eventDispatcherMock.Verify(e => e.Check(It.Is<PayPalAccountDeletingEvent>(x => x.PayPalKeyId == form.PayPalKeyId)), Times.Once);
        }
        #endregion

        #region SavePayPalKey() tests
        [Fact]
        public void SavePayPalKey_NewPayPalKeyNoAccessToPayPalKey_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var payPalKey = new PayPalKey();
            convertorMock.Setup(e => e.ConvertToPayPalKey(It.IsAny<PayPalKeyForm>())).Returns(payPalKey);
            identityResolverMock.SetupCheckEntitleForPayPalKey().Throws(new EntitleAccessDeniedException("", Entitle.CorePayPalKeys));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SavePayPalKey(new PayPalKeyForm()));
            identityResolverMock.VerifyCheckEntitleForPayPalKey(Entitle.CorePayPalKeys, payPalKey, Times.Once());
        }

        [Fact]
        public void SavePayPalKey_PayPalKeyInDbNoAccessToPayPalKey_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var payPalKey = new PayPalKey
            {
                PayPalKeyId = 15
            };
            convertorMock.Setup(e => e.ConvertToPayPalKey(It.IsAny<PayPalKeyForm>())).Returns(payPalKey);
            identityResolverMock.SetupCheckEntitleForPayPalKey().Throws(new EntitleAccessDeniedException("", Entitle.CorePayPalKeys));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SavePayPalKey(new PayPalKeyForm()));
            identityResolverMock.VerifyCheckEntitleForPayPalKey(Entitle.CorePayPalKeys, payPalKey, Times.Once());
        }

        [Fact]
        public void SavePayPalKey_PayPalKeyForm_ReturnsPayPalKeyIdAndSavesPayPalKey()
        {
            // arrange
            var payPalKey = new PayPalKey();
            var form = new PayPalKeyForm();
            convertorMock.Setup(e => e.ConvertToPayPalKey(It.IsAny<PayPalKeyForm>())).Returns(payPalKey);
            paymentDbMock.Setup(e => e.InsertPayPalKey(It.IsAny<PayPalKey>())).Returns(21);
            var admin = CreateAdmin();

            // act
            var result = admin.SavePayPalKey(form);

            // assert
            Assert.Equal(21, result);
            convertorMock.Verify(e => e.ConvertToPayPalKey(form), Times.Once);
            paymentDbMock.Verify(e => e.InsertPayPalKey(payPalKey), Times.Once);
        }

        [Fact]
        public void SavePayPalKey_PayPalKeyNotInDb_ThrowsArgumentException()
        {
            // arrange
            var form = new PayPalKeyForm
            {
                PayPalKeyId = 13
            };
            var payPalKey = new PayPalKey();
            convertorMock.Setup(e => e.ConvertToPayPalKey(It.IsAny<PayPalKeyForm>())).Returns(payPalKey);
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns((PayPalKey)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.SavePayPalKey(form));
            paymentDbMock.Verify(e => e.GetPayPalKeyById(13), Times.Once);
        }

        [Fact]
        public void SavePayPalKey_PayPalKeyForm_UpdatesPayPalKeyInDb()
        {
            // arrange
            var form = new PayPalKeyForm
            {
                PayPalKeyId = 13
            };
            var payPalKey = new PayPalKey();
            convertorMock.Setup(e => e.ConvertToPayPalKey(It.IsAny<PayPalKeyForm>())).Returns(payPalKey);
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(payPalKey);
            var admin = CreateAdmin();

            // act
            var result = admin.SavePayPalKey(form);

            // assert
            Assert.Equal(13, result);
            paymentDbMock.Verify(e => e.UpdatePayPalKey(payPalKey), Times.Once);
        }
        #endregion

        #region DeletePayPalKey() tests
        [Fact]
        public void DeletePayPalKey_NoAccessToPayPalKey_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var key = new PayPalKey
            {
                PayPalKeyId = 1
            };
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(key);
            identityResolverMock.SetupCheckEntitleForPayPalKey().Throws(new EntitleAccessDeniedException("", Entitle.CorePayPalKeys));
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.DeletePayPalKey(1));
            identityResolverMock.VerifyCheckEntitleForPayPalKey(Entitle.CorePayPalKeys, key, Times.Once());
        }

        [Fact]
        public void DeletePayPalKey_PayPalKeyCannotBeDeleted_ThrowsInvalidOperationException()
        {
            // arrange
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(new PayPalKey());
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PayPalAccountDeletingEvent>())).Returns(false);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.DeletePayPalKey(1));
            eventDispatcherMock.Verify(e => e.Check(It.Is<PayPalAccountDeletingEvent>(x => x.PayPalKeyId == 1)), Times.Once);
        }

        [Fact]
        public void DeletePayPalKey_PayPalKeyCanBeDeleted_DeletePayPalKey()
        {
            // arrange
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(new PayPalKey());
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PayPalAccountDeletingEvent>())).Returns(true);
            var admin = CreateAdmin();

            // act
            admin.DeletePayPalKey(1);

            // assert
            eventDispatcherMock.Verify(e => e.Check(It.Is<PayPalAccountDeletingEvent>(x => x.PayPalKeyId == 1)), Times.Once);
            paymentDbMock.Verify(e => e.DeletePayPalKey(1), Times.Once);
        }
        #endregion


        #region private methods
        private PaymentAdministration CreateAdmin()
        {
            return new PaymentAdministration(
                paymentDbMock.Object,
                identityResolverMock.Object,
                eventDispatcherMock.Object,
                convertorMock.Object);
        }
        #endregion
    }
}
