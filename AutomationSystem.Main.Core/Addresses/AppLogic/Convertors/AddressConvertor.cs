using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System;

namespace AutomationSystem.Main.Core.Addresses.AppLogic.Convertors
{

    /// <summary>
    /// Converts address objects
    /// </summary>
    public class AddressConvertor : IAddressConvertorLocalised
    {

        // determines empty field code
        private const string emptyField = "#EMPTY";


        private readonly ILocalisationService localisationService;
        private readonly bool useLocalisation;

        // constructor
        public AddressConvertor(ILocalisationService localisationService = null)
        {
            this.localisationService = localisationService;
            useLocalisation = localisationService != null;
        }


        // clones address
        public Address CloneAddress(Address address, bool forRegistration)
        {
            var result = new Address
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                Street2 = address.Street2,
                City = address.City,
                State = address.State,
                CountryId = address.CountryId,
                ZipCode = address.ZipCode,
                ForRegistration = forRegistration,
                IsIncomplete = address.IsIncomplete
            };
            return result;
        }


        // converts form address to db address
        public Address ConvertToAddress(AddressForm addressForm, bool forRegistration)
        {
            var result = new Address
            {
                FirstName = addressForm.FirstName,
                LastName = addressForm.LastName,
                Street = addressForm.Street,
                Street2 = addressForm.Street2,
                City = addressForm.City,
                State = addressForm.State,
                CountryId = addressForm.CountryId ?? 0,
                ZipCode = addressForm.ZipCode,
                ForRegistration = forRegistration,
                IsIncomplete = false
            };
            return result;
        }


        // converts incomplete form address to db address
        public Address ConvertToAddress(IncompleteAddressForm addressForm, bool forRegistration)
        {
            var result = new Address
            {
                FirstName = addressForm.FirstName,
                LastName = ToDbString(addressForm.LastName),
                Street = ToDbString(addressForm.Street),
                Street2 = addressForm.Street2,
                City = ToDbString(addressForm.City),
                State = addressForm.State,
                CountryId = addressForm.CountryId ?? 0,
                ZipCode = ToDbString(addressForm.ZipCode),
                ForRegistration = forRegistration,
                IsIncomplete = true
            };
            return result;
        }

               
        // converts db address to form address
        public AddressForm ConvertToAddressForm(Address address)
        {
            var result = new AddressForm
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                Street2 = address.Street2,
                City = address.City,
                State = address.State,
                CountryId = address.CountryId,
                ZipCode = address.ZipCode,
            };
            return result;
        }


        // converts db address to incomplete  form address
        public IncompleteAddressForm ConvertToIncompleteAddressForm(Address address)
        {
            var result = new IncompleteAddressForm
            {
                FirstName = address.FirstName,
                LastName = ToLogicString(address.LastName),
                Street = ToLogicString(address.Street),
                Street2 = address.Street2,
                City = ToLogicString(address.City),
                State = address.State,
                CountryId = address.CountryId,
                ZipCode = ToLogicString(address.ZipCode),
            };
            return result;
        }


        // converts db address to address detail
        public AddressDetail ConvertToAddressDetail(Address address)
        {
            if (address.Country == null)
                throw new InvalidOperationException("Country is not included into Address object");

            var result = new AddressDetail
            {
                AddressId = address.AddressId,
                FirstName = address.FirstName,
                LastName = ToLogicString(address.LastName),
                Street = ToLogicString(address.Street),
                Street2 = address.Street2,
                City = ToLogicString(address.City),
                State = address.State,
                CountryId = address.CountryId,
                Country = useLocalisation 
                    ? localisationService.GetLocalisedEnumItem(EnumTypeEnum.Country, (int) address.CountryId).Description
                    : address.Country.Description,
                ZipCode = ToLogicString(address.ZipCode)
            };
            result.FullName = MainTextHelper.GetFullName(result.FirstName, result.LastName);
            result.FullStreet = MainTextHelper.GetAddressStreet(result.Street, result.Street2);
            result.FullCity = MainTextHelper.GetAddressCityState(result.City, result.State, result.ZipCode);

            return result;
        }


        // updates db address
        public void UpdateAddresss(Address toUpdate, Address newAddress)
        {                  
            toUpdate.FirstName = newAddress.FirstName;
            toUpdate.LastName = newAddress.LastName;
            toUpdate.Street = newAddress.Street;
            toUpdate.Street2 = newAddress.Street2;
            toUpdate.City = newAddress.City;
            toUpdate.State = newAddress.State;
            toUpdate.CountryId = newAddress.CountryId;
            toUpdate.ZipCode = newAddress.ZipCode;
            toUpdate.ForRegistration = newAddress.ForRegistration;
            toUpdate.IsIncomplete = newAddress.IsIncomplete;
        }


        #region static methods

        // converts app logic string to db string - serves for incomplete address converting
        public static string ToDbString(string logicString)
        {
            return logicString ?? emptyField;
        }

        // converts db string to app logic string - serves for incomplete address converting
        public static string ToLogicString(string dbString)
        {
            return dbString == emptyField ? null : dbString;
        }

        #endregion
       
    }

}
