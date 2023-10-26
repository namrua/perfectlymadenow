using System;
using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Core.Incidents.System.MappingProfiles;
using AutomationSystem.Shared.Model;
using Xunit;

namespace AutomationSystem.Shared.Tests.Incidents.System.MappingProfiles
{
    public class IncidentProfileTests
    {
        #region CreateMap<Incident, IncidentDetail>()

        [Fact]
        public void Map_IncidentEntityTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var incident = CreateIncident();
            incident.EntityType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<IncidentDetail>(incident));
        }

        [Fact]
        public void Map_IncidentIncidentTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var incident = CreateIncident();
            incident.IncidentType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<IncidentDetail>(incident));
        }

        [Fact]
        public void Map_IncidentIncidentChildrenIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var incident = CreateIncident();
            incident.IncidentChildren = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<IncidentDetail>(incident));
        }

        [Fact]
        public void Map_Incident_ReturnsIncidentDetail()
        {
            // arrange
            var incident = CreateIncident();
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<IncidentDetail>(incident);

            // assert
            Assert.Equal(1, result.IncidentId);
            Assert.Equal(2, result.ParentIncidentId);
            Assert.Equal(3, result.InnerIncidentsCount);
            Assert.Equal(EntityTypeEnum.CoreEmail, result.EntityTypeId);
            Assert.Equal("Email", result.EntityType);
            Assert.Equal(100, result.EntityId);
            Assert.Equal(IncidentTypeEnum.EmailError, result.IncidentTypeId);
            Assert.Equal("Email error",result.IncidentType);
            Assert.Equal("Message", result.Message);
            Assert.Equal("Description", result.Description);
            Assert.Equal(new DateTime(2021, 1, 1), result.Occurred);
            Assert.True(result.CanBeReported);
            Assert.Equal(4, result.ReportingAttempts);
            Assert.True(result.IsReported);
            Assert.Equal(new DateTime(2021, 2, 2), result.Reported);
            Assert.True(result.IsResolved);
            Assert.Equal(new DateTime(2021, 3, 3), result.Resolved);
        }

        #endregion

        #region  CreateMap<IncidentForLog, IncidentDetail>()

        [Fact]
        public void Map_IncidentForLog_ReturnsIncident()
        {
            // arrange
            var incidentForLog = new IncidentForLog
            {
                Message = "Message",
                Description = "Description",
                IpAddress = "IpAddress",
                RequestUrl = "RequestUrl",
                CanBeReported = true,
                EntityTypeId = EntityTypeEnum.CoreEmail,
                EntityId = 100,
                Occurred = new DateTime(2021, 1, 1),
                IncidentTypeId = IncidentTypeEnum.EmailError
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<Incident>(incidentForLog);

            // assert
            Assert.Equal("Message", result.Message);
            Assert.Equal("Description", result.Description);
            Assert.Equal("IpAddress", result.IpAddress);
            Assert.Equal("RequestUrl", result.RequestUrl);
            Assert.True(result.CanBeReport);
            Assert.Equal(EntityTypeEnum.CoreEmail, result.EntityTypeId);
            Assert.Equal(100, result.EntityId);
            Assert.Equal(new DateTime(2021, 1, 1), result.Occurred);
            Assert.Equal(IncidentTypeEnum.EmailError, result.IncidentTypeId);

            Assert.Empty(result.IncidentChildren);
        }

        [Fact]
        public void Map_IncidentForLogWithChildren_ReturnsIncidentWithChildren()
        {
            // arrange
            var incidentForLog = IncidentForLog.New(IncidentTypeEnum.EmailError, "Parent")
                .AddChild(IncidentForLog.New(IncidentTypeEnum.EmailError, "Child1"))
                .AddChild(IncidentForLog.New(IncidentTypeEnum.EmailError, "Child2"));
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<Incident>(incidentForLog);

            // assert
            Assert.Collection(
                result.IncidentChildren,
                item => Assert.Equal("Child1", item.Message),
                item => Assert.Equal("Child2", item.Message));
        }

        #endregion

        #region private methods

        private Incident CreateIncident()
        {
            var result = new Incident
            {
                IncidentId = 1,
                ParentIncidentId = 2,
                EntityTypeId = EntityTypeEnum.CoreEmail,
                EntityType = new EntityType
                {
                    Description = "Email"
                },
                EntityId = 100,
                IncidentTypeId = IncidentTypeEnum.EmailError,
                IncidentType = new IncidentType
                {
                    Description = "Email error"
                },
                Message = "Message",
                Description = "Description",
                Occurred = new DateTime(2021, 1, 1),
                CanBeReport = true,
                IsReported = true,
                ReportingAttempts = 4,
                Reported = new DateTime(2021, 2, 2),
                IsResolved = true,
                Resolved = new DateTime(2021, 3, 3),
                IncidentChildren = new List<Incident>
                {
                    new Incident(),
                    new Incident(),
                    new Incident()
                }
            };

            return result;
        }

        private Mapper CreateMapper()
        {
            var mapConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new IncidentProfile());
            });
            return new Mapper(mapConfiguration);
        }

        #endregion
    }
}
