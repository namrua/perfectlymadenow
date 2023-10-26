using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;
using PerfectlyMadeInc.WebEx.Contract.Connectors;

namespace PerfectlyMadeInc.WebEx.Connectors.Integration
{
    /// <summary>
    /// WebEx provider
    /// </summary>
    public class WebExProvider : IWebExProvider
    {

        // private fields
        private readonly WebExSettingInfo settings;

        // private fields
        private readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private readonly XNamespace serv = "http://www.webex.com/schemas/2002/06/service";
        private readonly XNamespace att = "http://www.webex.com/schemas/2002/06/service/attendee";
        private readonly XNamespace eve = "http://www.webex.com/schemas/2002/06/service/event";
        private readonly XNamespace com = "http://www.webex.com/schemas/2002/06/common";

        private const string encoding = "ISO-8859-1";

        // constructor
        public WebExProvider(WebExSettingInfo settings)
        {
            this.settings = settings;
        }

        // constructor
        public WebExProvider() { }


        #region IWebExProvider interface methods

        // gets list of persons in session
        public async Task<List<WebExPersonExtended>> GetPersonsInSession(long sessionId)
        {
            var result = new List<WebExPersonExtended>();
            var status = new ResponseStatus();

            CheckIsEnabled();
            try
            {
                var body = GetBodyForListOfPersons(sessionId);
                var response = await GetResponse(body);
                var xmlResponse = ConvertResponseToXml(response);
                status = GetConvertedReponse(xmlResponse);
                CheckResponseStatus(status, xmlResponse);

                result = GetListOfPersons(xmlResponse);
            }
            catch (WebExException)
            {
                if (status.ErrorMessage == "Sorry, no record found")
                    return result;
                else
                    throw;
            }
            return result;
        }


        // gets person by id
        public async Task<WebExPersonExtended> GetPersonById(long sessionId, long attendeeId)
        {           
            var persons = await GetPersonsInSession(sessionId);
            var result = persons.FirstOrDefault(x => x.PersonInfo.AttendeeId == attendeeId);
            return result;
        }


        // gets list of programs
        public async Task<List<WebExProgramInfo>> GetPrograms()
        {
            CheckIsEnabled();
            var body = GetBodyForListOfPrograms();
            var response = await GetResponse(body);
            var xmlResponse = ConvertResponseToXml(response);
            var status = GetConvertedReponse(xmlResponse);
            CheckResponseStatus(status, xmlResponse);

            var result = GetListOfPrograms(xmlResponse);
            return result;
        }

        
        // gets list of events by program id
        public async Task<List<WebExEventInfo>> GetsEventsByProgramId(long programId)
        {
            var result = new List<WebExEventInfo>();
            var status = new ResponseStatus();
            try
            {
                CheckIsEnabled();
                var body = GetBodyForListOfEvents(programId);
                var response = await GetResponse(body);
                var xmlResponse = ConvertResponseToXml(response);
                status = GetConvertedReponse(xmlResponse);
                CheckResponseStatus(status, xmlResponse);
                result = GetListOfEvents(xmlResponse);

            }
            catch(WebExException)
            {
                if (status.ErrorMessage == "Sorry, no record found")
                    return result;
                else
                    throw;
            }            
            return result;
        }

        // gets program by id
        public async Task<WebExProgramInfo> GetProgramById(long programId)
        {
            WebExProgramInfo result;
            var status = new ResponseStatus();
            try
            {
                CheckIsEnabled();              
                var body = GetBodyForListOfPrograms(programId);
                var response = await GetResponse(body);
                var xmlResponse = ConvertResponseToXml(response);
                status = GetConvertedReponse(xmlResponse);
                CheckResponseStatus(status, xmlResponse);

                result = GetProgramInfo(xmlResponse);
            }
            catch (WebExException)
            {
                if (status.ErrorMessage == "Sorry, no record found")
                    return null;
                else
                    throw;
            }
            return result;
        }

        // add or update person into/in WebEx session - returns attendeeId
        public async Task<long> SavePerson(long sessionId, WebExPersonExtended person)
        {
            CheckIsEnabled();
            var body = GetBodyForSave(person, sessionId);
            var response = await GetResponse(body);
            var xmlResponse = ConvertResponseToXml(response);
            var status = GetConvertedReponse(xmlResponse);
            CheckResponseStatus(status, xmlResponse);

            var result = GetAttendeeId(xmlResponse);
            return result;
        }

        // delete person from session by attendeeId - returns true whether person was deleted
        public async Task<bool> DeletePersonByAttendeeId(long attendeeId)
        {
            CheckIsEnabled();
            var body = GetBodyForDelete(attendeeId);
            var response = await GetResponse(body);
            var xmlResponse = ConvertResponseToXml(response);
            var status = GetConvertedReponse(xmlResponse);

            if (status.IsSuccess) return true;
            if (status.ErrorMessage.Equals("Meeting attendee not found"))
                return false;
            CheckResponseStatus(status, xmlResponse);
            return false;
        }
       
        #endregion


        #region request creators

        // wraps requests to common envelope
        private XDocument WrapRequest(XElement body)
        {
            XNamespace webex = "http://www.webex.com";
            var root = new XDocument(
                new XDeclaration("1.0", encoding, "yes"),
                new XElement(webex + "message",
                            new XAttribute(XNamespace.Xmlns + "serv", "http://www.webex.com"),
                            new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XElement("header",
                                    new XElement("securityContext",
                                        new XElement("webExID", settings.Login),
                                        new XElement("password", settings.Password),
                                        new XElement("siteName", settings.SiteName)
                                        )),
                                        body));
            return root;
        }

        // gets body for list of persons request
        private XElement GetBodyForListOfPersons(long sessionId)
        {
            var result = new XElement("body",
                        new XElement("bodyContent",
                            new XAttribute(xsi + "type", "java:com.webex.service.binding.attendee.LstMeetingAttendee"),
                            new XElement("listControl", 
                                new XElement("maximumNum", 500)),
                            new XElement("sessionKey", sessionId
                            )));
            return result;
        }

        // gets body for list of persons request
        private XElement GetBodyForListOfPrograms()
        {
            var result = new XElement("body",
                        new XElement("bodyContent",
                            new XAttribute(xsi + "type", "java:com.webex.service.binding.event.LstsummaryProgram")
                            ));
            return result;
        }

        // gets body for list of persons request
        private XElement GetBodyForListOfPrograms(long programId)
        {
            var result = new XElement("body",
                        new XElement("bodyContent",
                            new XAttribute(xsi + "type", "java:com.webex.service.binding.event.LstsummaryProgram"),
                            new XElement("programID", programId)
                            ));
            return result;
        }

        // gets body for list of events request
        private XElement GetBodyForListOfEvents(long programId)
        {
            var result = new XElement("body",
                        new XElement("bodyContent",
                            new XAttribute(xsi + "type", "java:com.webex.service.binding.event.LstsummaryEvent"),
                                new XElement("programID", programId)
                            ));
            return result;
        }

        // gets body for save request
        private XElement GetBodyForSave(WebExPersonExtended person, long sessionId)
        {
            var result = new XElement("body",
                        new XElement("bodyContent",
                            new XAttribute(xsi + "type", "java:com.webex.service.binding.attendee.RegisterMeetingAttendee"),
                            new XElement("attendees",
                                new XElement("person",
                                    new XElement("firstName", person.PersonInfo.FirstName),
                                    new XElement("lastName", person.PersonInfo.LastName),
                                    new XElement("email", person.PersonInfo.Email),
                                    new XElement("type", GetStringPersonTypeEnum(person.AdditionalInfo.PersonType)),
                                    new XElement("address",
                                        new XElement("address1", person.PersonInfo.Street),
                                        new XElement("address2", person.PersonInfo.Street2),
                                        new XElement("state", person.PersonInfo.State),
                                        new XElement("city", person.PersonInfo.City),
                                        new XElement("zipCode", person.PersonInfo.ZipCode),
                                        new XElement("country", person.PersonInfo.Country))),
                                new XElement("joinStatus", GetStringStatusEnum(person.AdditionalInfo.Status)),
                                new XElement("role", GetStringRoleEnum(person.AdditionalInfo.Role)),
                                new XElement("emailInvitations", person.AdditionalInfo.SendEmailInvitation),
                                new XElement("sessionKey", sessionId))));
            return result;
        }

        // gets body for delet request
        private XElement GetBodyForDelete(long attendeeId)
        {
            var result = new XElement("body",
                        new XElement("bodyContent",
                            new XAttribute(xsi + "type", "java:com.webex.service.binding.attendee.DelMeetingAttendee"),
                            new XElement("attendeeID", attendeeId
                            )));
            return result;
        }

        #endregion


        #region calling execution methods

        // calls WebEx service and gets response
        private async Task<string> GetResponse(XElement body)
        {
            using (var client = new HttpClient())
            {
                var xml = WrapRequest(body);
                var request = xml.Declaration + "\r\n" + xml;
                var content = new StringContent(request, Encoding.GetEncoding(encoding), "application/xml");
                var response = await client.PostAsync(settings.ServiceUrl, content);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        // converts response string to wrapped xml response
        private XmlResponseWrapper ConvertResponseToXml(string response)
        {
            var result = new XmlResponseWrapper();
            var reader = XmlReader.Create(new StringReader(response));
            result.Root = XElement.Load(reader);
            var nameTable = reader.NameTable;
            result.NamespaceManager = new XmlNamespaceManager(nameTable ?? throw new InvalidOperationException());
            result.NamespaceManager.AddNamespace("com", "http://www.webex.com/schemas/2002/06/common");
            result.NamespaceManager.AddNamespace("serv", "http://www.webex.com/schemas/2002/06/service");
            result.NamespaceManager.AddNamespace("att", "http://www.webex.com/schemas/2002/06/service/attendee");
            result.NamespaceManager.AddNamespace("event", "http://www.webex.com/schemas/2002/06/service/event");
            return result;
        }

        // checks response status
        private void CheckResponseStatus(ResponseStatus status, XmlResponseWrapper xml)
        {
            if (!status.IsSuccess)
                throw new WebExException(status.ErrorMessage, xml.Root.ToString());
        }

        #endregion


        #region response parsing methods

        // gets list of person from xml
        private List<WebExPersonExtended> GetListOfPersons(XmlResponseWrapper response)
        {
            var attendees = response.Root.XPathSelectElements("./serv:body/serv:bodyContent/att:attendee", response.NamespaceManager);
            var personList = new List<WebExPersonExtended>();
            foreach (var member in attendees)
            {
                var attendee = new WebExPersonExtended();
                var personInfo = attendee.PersonInfo;
                var additional = attendee.AdditionalInfo;
                var personElem = member.Element(att + "person");
                if (personElem != null)
                {
                    personInfo.FirstName = GetElemValue(personElem, com + "firstName");
                    personInfo.LastName = GetElemValue(personElem, com + "lastName");
                    personInfo.Email = GetElemValue(personElem, com + "email");
                    additional.PersonType = GetPersonTypeEnum(GetElemValue(personElem, com + "type"));

                    var addrElem = personElem.Element(com + "address");
                    if (addrElem != null)
                    {
                        personInfo.Street = GetElemValue(addrElem, com + "address1");
                        personInfo.Street2 = GetElemValue(addrElem, com + "address2");
                        personInfo.State = GetElemValue(addrElem, com + "state");
                        personInfo.City = GetElemValue(addrElem, com + "city");
                        personInfo.ZipCode = GetElemValue(addrElem, com + "zipCode");
                        personInfo.Country = GetElemValue(addrElem, com + "country");
                    }
                }
                additional.Role = GetRoleEnum(GetElemValue(member, att + "role"));
                additional.Status = GetStatusEnum(GetElemValue(member, att + "joinStatus"));
                personInfo.AttendeeId = Convert.ToInt64(GetElemValue(member, att + "attendeeId"));
                personList.Add(attendee);
            }
            return personList;
        }

        // gets list of events from xml
        private List<WebExEventInfo> GetListOfEvents(XmlResponseWrapper response)
        {
            var events = response.Root.XPathSelectElements("./serv:body/serv:bodyContent/event:event", response.NamespaceManager);
            var result = new List<WebExEventInfo>();
            foreach (var eventEntry in events)
            {
                var newEvent = new WebExEventInfo();
                newEvent.SessionId = Convert.ToInt64(GetElemValue(eventEntry, eve + "sessionKey"));
                newEvent.SessionName = GetElemValue(eventEntry, eve + "sessionName");
                result.Add(newEvent);
            }
            return result;
        }

        // gets list of programs from xml
        private WebExProgramInfo GetProgramInfo(XmlResponseWrapper response)
        {
            var program = response.Root.XPathSelectElements("./serv:body/serv:bodyContent/event:program", response.NamespaceManager).First();
            var result = new WebExProgramInfo();
            result.ProgramId = Convert.ToInt64(GetElemValue(program, eve + "programID"));
            result.Name = GetElemValue(program, eve + "programName");
            result.ProgramUrl = GetElemValue(program, eve + "programURL");            
            return result;
        }

        // gets list of programs from xml
        private List<WebExProgramInfo> GetListOfPrograms(XmlResponseWrapper response)
        {
            var programs = response.Root.XPathSelectElements("./serv:body/serv:bodyContent/event:program", response.NamespaceManager);
            var result = new List<WebExProgramInfo>();
            foreach (var programEntry in programs)
            {
                var program = new WebExProgramInfo();
                program.ProgramId = Convert.ToInt64(GetElemValue(programEntry, eve + "programID"));
                program.Name = GetElemValue(programEntry, eve + "programName");
                program.ProgramUrl = GetElemValue(programEntry, eve + "programURL");
                result.Add(program);
            }
            return result;
        }

        // converts xml to response status
        private ResponseStatus GetConvertedReponse(XmlResponseWrapper response)
        {
            var result = new ResponseStatus();
            var responseElem = response.Root.XPathSelectElement("./serv:header/serv:response", response.NamespaceManager);

            if (responseElem != null)
            {
                string isSuccess = GetElemValue(responseElem, serv + "result");
                result.IsSuccess = isSuccess == "SUCCESS";
                result.ErrorMessage = GetElemValue(responseElem, serv + "reason");
            }
            return result;
        }

        // gets attendeeId from response message
        private long GetAttendeeId(XmlResponseWrapper response)
        {
            var register = response.Root.XPathSelectElement("./serv:body/serv:bodyContent/att:register", response.NamespaceManager);
            var result = GetElemValue(register, att + "attendeeID");
            return (result == null) ? 0 : Convert.ToInt64(result);
        }


        // gets element value by elemName (or null if element does not exist)
        private string GetElemValue(XElement personElem, XName elemName)
        {
            var elem = personElem.Element(elemName);
            return elem?.Value;
        }

        #endregion


        #region enums convertors

        private WebExRole GetRoleEnum(string role)
        {
            switch (role)
            {
                case "HOST": return WebExRole.Host;
                case "PRESENTER": return WebExRole.Presenter;
                case "ATTENDEE": return WebExRole.Attendee;
                default: throw new ArgumentException($"Unknown webex role type {role}.");
            }
        }

        private WebExPersonType GetPersonTypeEnum(string type)
        {
            switch (type)
            {
                case "MEMBER": return WebExPersonType.Member;
                case "VISITOR": return WebExPersonType.Visitor;
                case "PANELIST": return WebExPersonType.Panelist;
                default: throw new ArgumentException($"Unknown webex person type {type}.");
            }
        }

        private WebExStatus GetStatusEnum(string status)
        {
            switch (status)
            {
                case "ACCEPT": return WebExStatus.Accept;
                case "INVITE": return WebExStatus.Invite;
                case "REJECT": return WebExStatus.Reject;
                case "REGISTER": return WebExStatus.Register;
                default: throw new ArgumentException($"Unknown webex status {status}.");
            }
        }

        private string GetStringRoleEnum(WebExRole role)
        {
            switch (role)
            {
                case WebExRole.Host: return "HOST";
                case WebExRole.Presenter: return "PRESENTER";
                case WebExRole.Attendee: return "ATTENDEE";
                default: throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }

        }

        private string GetStringPersonTypeEnum(WebExPersonType type)
        {
            switch (type)
            {
                case WebExPersonType.Member: return "MEMBER";
                case WebExPersonType.Visitor: return "VISITOR";
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private string GetStringStatusEnum(WebExStatus status)
        {
            switch (status)
            {
                case WebExStatus.Accept: return "ACCEPT";
                case WebExStatus.Invite: return "INVITE";
                case WebExStatus.Reject: return "REJECT";
                case WebExStatus.Register: return "REGISTER";
                default: throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        #endregion

        #region misc

        // checks whether integrator is enabled
        private void CheckIsEnabled()
        {
            if (!settings.Enabled)
                throw new InvalidOperationException("WebEx integration is not enabled.");
        }

      
        #endregion


    }
}
