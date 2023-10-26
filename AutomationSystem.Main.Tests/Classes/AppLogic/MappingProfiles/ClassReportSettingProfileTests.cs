using AutoMapper;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Reports;
using AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles;
using AutomationSystem.Main.Model;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.MappingProfiles
{
    public class ClassReportSettingProfileTests
    {
        #region CreateMap<ClassPreference, ClassReportSetting>() tests
        [Fact]
        public void Map_ClassPreference_ReturnsClassReportSetting()
        {
            // arrange
            var preference = new ClassPreference
            {
                VenueName = "VenueName",
                LocationCode = "LocationCode",
                LocationInfoId = 1,
                LocationInfo = new Person()
            };
            var mapper = CreateMapper();

            // act
            var setting = mapper.Map<ClassReportSetting>(preference);

            // arrange
            Assert.Equal("VenueName", setting.VenueName);
            Assert.Equal("LocationCode", setting.LocationCode);
            Assert.Equal(1, setting.LocationInfoId);
            Assert.Null(setting.LocationInfo);
        }
        #endregion

        #region CreateMap<ClassReportSetting, ClassReportSettingForm>() tests
        [Fact]
        public void Map_ClassReportSetting_ReturnsClassReportSettingForm()
        {
            // arrange
            var setting = new ClassReportSetting
            {
                VenueName = "VenueName",
                LocationCode = "LocationCode",
                LocationInfoId = 1,
            };
            var mapper = CreateMapper();

            // act
            var form = mapper.Map<ClassReportSettingForm>(setting);

            // assert
            Assert.Equal("VenueName", form.VenueName);
            Assert.Equal("LocationCode", form.LocationCode);
            Assert.Equal(1, form.LocationInfoId);
        }
        #endregion

        #region CreateMap<ClassReportSettingForm, ClassReportSetting>() tests
        [Fact]
        public void Map_ClassReportSettingForm_ReturnsClassReportSetting()
        {
            // arrange
            var form = new ClassReportSettingForm
            {
                VenueName = "VenueName",
                LocationCode = "LocationCode",
                LocationInfoId = 1
            };
            var mapper = CreateMapper();

            // act
            var setting = mapper.Map<ClassReportSetting>(form);

            // arrange
            Assert.Equal("VenueName", setting.VenueName);
            Assert.Equal("LocationCode", setting.LocationCode);
            Assert.Equal(1, setting.LocationInfoId);
        }
        #endregion

        #region private methods
        private Mapper CreateMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
               cfg.AddProfile(new ClassReportSettingProfile());
            });
            return new Mapper(mapperCfg);
        }
        #endregion
    }
}
