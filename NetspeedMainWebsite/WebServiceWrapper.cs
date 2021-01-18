using NetspeedMainWebsite.MainSiteServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NetspeedMainWebsite
{
    public class WebServiceWrapper
    {
        private readonly string Username;

        private readonly string PasswordHash;

        private readonly string Culture;

        private readonly string Rand;

        private readonly MainSiteServiceClient InternalClient;

        public WebServiceWrapper()
        {
            Username = Properties.Settings.Default.WebServiceUsername;
            PasswordHash = HashUtilities.HashCalculate(Properties.Settings.Default.WebServicePasswordHash);
            Culture = "tr-tr";
            Rand = Guid.NewGuid().ToString("N");
            InternalClient = new MainSiteServiceClient();
        }
        private string CalculateHash()
        {
            var k=InternalClient.GetKeyFragment(Username);
            var hashedHexString = HashUtilities.HashCalculate($"{Username}{Rand}{PasswordHash}{k}");
            return hashedHexString;
        }

        public NetspeedServiceAddressDetailsResponse GetApartmentAddress(long? BBKCode)
        {
            return InternalClient.GetApartmentAddress(new NetspeedServiceAddressDetailsRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                BBK = BBKCode,
            });
        }

        public NetspeedServiceArrayListResponse GetProvinces()
        {
            return InternalClient.GetProvinces(new NetspeedServiceRequests()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture
            });
        }

        public NetspeedServiceArrayListResponse GetProvinceDistricts(long? code)
        {
            return InternalClient.GetProvinceDistricts(new NetspeedServiceArrayListRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ItemCode = code

            });
        }

        public NetspeedServiceArrayListResponse GetDistrictRuralRegions(long? code)
        {
            return InternalClient.GetDistrictRuralRegions(new NetspeedServiceArrayListRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ItemCode = code
            });
        }

        public NetspeedServiceArrayListResponse GetRuralRegionNeighbourhoods(long? code)
        {
            return InternalClient.GetRuralRegionNeighbourhoods(new NetspeedServiceArrayListRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ItemCode = code
            });
        }
        public NetspeedServiceArrayListResponse GetNeighbourhoodStreets(long? code)
        {
            return InternalClient.GetNeighbourhoodStreets(new NetspeedServiceArrayListRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ItemCode = code
            });
        }
        public NetspeedServiceArrayListResponse GetStreetBuildings(long? code)
        {
            return InternalClient.GetStreetBuildings(new NetspeedServiceArrayListRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ItemCode = code
            });
        }

        public NetspeedServiceArrayListResponse GetBuildingApartments(long? code)
        {
            return InternalClient.GetBuildingApartments(new NetspeedServiceArrayListRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ItemCode = code
            });
        }


        public NetspeedServiceArrayListResponse GetIDCardTypes()
        {
            return InternalClient.GetIDCardTypes(new NetspeedServiceRequests()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture
            });
        }

        public NetspeedServiceArrayListResponse GetSexes()
        {
            return InternalClient.GetSexes(new NetspeedServiceRequests()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture
            });
        }
        public NetspeedServiceSubscriberGetBillsResponse GetBills(string PhoneNumber, string ClientInfo)
        {
            return InternalClient.GetBills(new NetspeedServiceSubscriberGetBillsRequest()
            {
                Username = Username,
                Rand = Rand,
                Culture = Culture,
                Hash = CalculateHash(),
                GetBillParameters= new SubscriberGetBillsRequest
                {
                    PhoneNo=PhoneNumber,
                    TCKOrSubscriberNo=ClientInfo
                }
            });
        }

        public NetspeedServiceArrayListResponse GetNationalities()
        {
            return InternalClient.GetNationalities(new NetspeedServiceRequests()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture
            });
        }

        public NetspeedServiceArrayListResponse GetProfessions()
        {
            return InternalClient.GetProfessions(new NetspeedServiceRequests()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture
            });
        }
         public NetspeedServiceExternalTariffResponse GetTariffList()
        {
            return InternalClient.ExternalTariffList(new NetspeedServiceExternalTariffRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture
                
            });
            //domainId-
            //service-tarife
        }

        


        public NetspeedServiceNewCustomerRegisterResponse NewCustomerRegister(/*int? BillingPeriod,*/ int? DomainID, int? ServiceID, long? ProvinceId, string ProvinceName,
            long? DistrictId, string DistrictName, long? RegionId, long? NeighbourhoodId, string NeighbourhoodName, long? StreetId, string StreetName, long? ApartmentId,
            string ApartmentNo, string AddressText, long? AddressNo, long? DoorId, string DoorNo, string Floor, int? PostalCode,
            string BirthPlace, string FathersName, string MothersMaidenName, string MothersName, int Nationality, int Profession, int Sex,
            DateTime? BirthDate, int? CardType, string FirstName, string LastName, string TCKNo, string SerialNo, string PlaceOfIssue, DateTime? DateOfIssue,
            string[] OtherPhoneNos, string ContactPhoneNo,
            string Culture, /*int? CustomerType, */string Email/*, string[] CorporateCustomerInfo*/, string ReferenceCode)
        {
            return InternalClient.NewCustomerRegister(new NetspeedServiceNewCustomerRegisterRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                CustomerRegisterParameters = new NewCustomerRegisterRequest()
                {
                    SubscriptionInfo = new SubscriptionRegistrationInfo()
                    {
                        ReferralDiscountInfo=new ReferralDiscountInfo()
                        { 
                            ReferenceNo=ReferenceCode
                        },
                        //BillingPeriod = BillingPeriod,
                        DomainID = DomainID,//tarifler al
                        ServiceID = ServiceID,//tarifelerden al
                        SetupAddress = new AddressInfo()
                        {
                            ProvinceID = ProvinceId,
                            ProvinceName = ProvinceName,
                            DistrictID = DistrictId,
                            DistrictName = DistrictName,
                            RuralCode = RegionId,
                            NeighbourhoodID = NeighbourhoodId,
                            NeighbourhoodName = NeighbourhoodName,
                            StreetID = StreetId,
                            StreetName = StreetName,
                            ApartmentID = ApartmentId,
                            ApartmentNo = ApartmentNo,
                            AddressNo = AddressNo,
                            AddressText = AddressText,
                            DoorID = DoorId,
                            DoorNo = DoorNo,
                            Floor = Floor,
                            PostalCode = PostalCode
                        }
                    },
                    IndividualCustomerInfo = new IndividualCustomerInfo()
                    {                        
                        BirthPlace = BirthPlace,
                        FathersName = FathersName/*"HÜSEYİN"*/,
                        MothersMaidenName = MothersMaidenName /*"KALAYCIOĞULLARI"*/,
                        MothersName = MothersName/*"GÜLŞAH"*/,
                        Nationality = Nationality,
                        Profession = Profession,
                        Sex = Sex,
                        ResidencyAddress = new AddressInfo()
                        {
                            ProvinceID = ProvinceId,
                            ProvinceName = ProvinceName,
                            DistrictID = DistrictId,
                            DistrictName = DistrictName,
                            RuralCode = RegionId,
                            NeighbourhoodID = NeighbourhoodId,
                            NeighbourhoodName = NeighbourhoodName,
                            StreetID = StreetId,
                            StreetName = StreetName,
                            ApartmentID = ApartmentId,
                            ApartmentNo = ApartmentNo,
                            AddressNo = AddressNo,
                            AddressText = AddressText,
                            DoorID = DoorId,
                            DoorNo = DoorNo,
                            Floor = Floor,
                            PostalCode = PostalCode,
                        },
                    },
                    IDCardInfo = new IDCardInfo()
                    {
                        BirthDate = BirthDate,  //ApplicationItemList[0].BirthDate,
                        CardType = CardType,
                        FirstName = FirstName,
                        LastName = LastName,
                        TCKNo = TCKNo,
                        SerialNo = SerialNo,
                        PlaceOfIssue = PlaceOfIssue,
                        DateOfIssue = DateOfIssue,
                    },
                    CustomerGeneralInfo = new CustomerGeneralInfo()
                    {
                        OtherPhoneNos = null,
                        BillingAddress = new AddressInfo()
                        {
                            ProvinceID = ProvinceId,
                            ProvinceName = ProvinceName,
                            DistrictID = DistrictId,
                            DistrictName = DistrictName,
                            RuralCode = RegionId,
                            NeighbourhoodID = NeighbourhoodId,
                            NeighbourhoodName = NeighbourhoodName,
                            StreetID = StreetId,
                            StreetName = StreetName,
                            ApartmentID = ApartmentId,
                            ApartmentNo = ApartmentNo,
                            AddressNo = AddressNo,
                            AddressText = AddressText,
                            DoorID = DoorId,
                            DoorNo = DoorNo,
                            Floor = Floor,
                            PostalCode = PostalCode,
                           
                        },
                        //ContactPhoneNo = ApplicationItemList[0].PhoneNumber,
                        ContactPhoneNo = ContactPhoneNo/*"5465939624"*/,
                        Culture = "tr-tr",
                        //CustomerType = application.CustomerType,
                        //CustomerType = 1,
                        Email = Email,

                        //OtherPhoneNos = new PhoneNoListItem()
                        //{
                        //    Number=
                        //}

                    },
                    CorporateCustomerInfo =null /*new CorporateCustomerInfo*/
                    //{
                        //CentralSystemNo = null,
                        //CompanyAddress = null,
                        //ExecutiveBirthPlace = null,
                        //ExecutiveFathersName = null,
                        //ExecutiveMothersMaidenName = null,
                        //ExecutiveNationality = null,
                        //ExecutiveMothersName = null,
                        //ExecutiveProfession = null,
                        //ExecutiveResidencyAddress = null,
                        //ExecutiveSex = null,
                        //TaxNo = null,
                        //TaxOffice = null,
                        //Title = null,
                        //ExtensionData = null
                    //}
                }
            }); 

        }

        public NetspeedServicePayBillsResponse PayBills(long[] billIds)
        {
            return InternalClient.PayBills(new NetspeedServicePayBillsRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                PayBillsParameters= billIds
            });
        }

        public NetspeedServiceRegisterCustomerContactResponse RegisterCustomerContact(string FullName, string PhoneNumber)
        {
            return InternalClient.RegisterCustomerContact(new NetspeedServiceCustomerContactRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                CustomerContactParameters= new CustomerContactRequest
                {
                    FullName=FullName,
                    PhoneNo=PhoneNumber,
                    RequestSubTypeID = 1048,
                    RequestTypeID = 1022,
                }
            });
        }

        //public NetspeedServiceRegisterSMSValidationResponse RegisterSMSValidation(string phoneNumber, string password)
        //{
        //    return InternalClient.RegisterSMSValidation(new NetspeedServiceRegisterSMSValidationRequest()
        //    {
        //        Username = Username,
        //        Rand = Rand,
        //        Hash = CalculateHash(),
        //        Culture = Culture,
        //        RegisterSMSValidationParameters = new RegisterSMSValidationRequest
        //        {
        //            CustomerPhoneNo = phoneNumber,
        //            Password = password
        //        }

        //    });
        //}


        //public NetspeedServiceRegisterSMSValidationResponse RegisterSMSValidation(string phoneNumber, string password)
        //{
        //    return InternalClient.RegisterSMSValidation(new NetspeedServiceRegisterSMSValidationRequest()
        //    {
        //        Username = Username,
        //        Rand = Rand,
        //        Hash = CalculateHash(),
        //        Culture = Culture,
        //        RegisterSMSValidationParameters = new RegisterSMSValidationRequest
        //        {
        //            CustomerPhoneNo = phoneNumber,
        //            Password = password
        //        }

        //    });
        //}

        public NetspeedServiceSendGenericSMSResponse SendGenericSMS(string phoneNumber)
        {
            return InternalClient.SendGenericSMS(new NetspeedServiceSendGenericSMSRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                SendGenericSMSParameters = new SendGenericSMSRequest
                {
                    CustomerPhoneNo = phoneNumber,
                    
                }
            });
        }

        public NetspeedServiceServiceAvailabilityResponse ServiceAvailability(string apartmentId)
        {
            return InternalClient.ServiceAvailability(new NetspeedServiceServiceAvailabilityRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                ServiceAvailabilityParameters = new ServiceAvailabilityRequest
                {
                    bbk = apartmentId
                }
            });
        }

        public NetspeedServicePaymentVPOSResponse SubscriberPaymentVPOS(long[] BillList, string FailUrl, string OkUrl)
        {
            return InternalClient.SubscriberPaymentVPOS(new NetspeedServicePaymentVPOSRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
               PaymentVPOSParameters= new PaymentVPOSRequest
               {
                   BillIds=BillList,
                   FailUrl=FailUrl,
                   OkUrl=OkUrl
               }
            });
        }
    }
}