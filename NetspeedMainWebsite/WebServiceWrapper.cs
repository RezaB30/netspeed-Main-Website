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

        private string Rand;

        private readonly MainSiteServiceClient InternalClient;

        public WebServiceWrapper()
        {
            Username = Properties.Settings.Default.WebServiceUsername;
            PasswordHash = HashUtilities.HashCalculate(Properties.Settings.Default.WebServicePasswordHash);
            Culture = "tr-tr";
            InternalClient = new MainSiteServiceClient();
        }
        private string CalculateHash()
        {
            var k = InternalClient.GetKeyFragment(Username);
            var hashedHexString = HashUtilities.HashCalculate($"{Username}{Rand}{PasswordHash}{k}");
            return hashedHexString;
        }

        private void UpdateRandom()
        {
            Rand = Guid.NewGuid().ToString("N");
        }

        public NetspeedServiceAddressDetailsResponse GetApartmentAddress(long? BBKCode)
        {
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
            return InternalClient.GetBills(new NetspeedServiceSubscriberGetBillsRequest()
            {
                Username = Username,
                Rand = Rand,
                Culture = Culture,
                Hash = CalculateHash(),
                GetBillParameters = new SubscriberGetBillsRequest
                {
                    PhoneNo = PhoneNumber,
                    TCKOrSubscriberNo = ClientInfo
                }
            });
        }

        public NetspeedServiceArrayListResponse GetNationalities()
        {
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
            return InternalClient.ExternalTariffList(new NetspeedServiceExternalTariffRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture

            });
        }


        public NetspeedServiceNewCustomerRegisterResponse NewCustomerRegister(/*int? BillingPeriod,*/ /*int? DomainID,*/ int? ServiceID, long? ProvinceId, string ProvinceName,
            long? DistrictId, string DistrictName, long? RegionId, long? NeighbourhoodId, string NeighbourhoodName, long? StreetId, string StreetName, long? ApartmentId,
            string ApartmentNo, string AddressText, long? AddressNo, long? DoorId, string DoorNo, string Floor, int? PostalCode,
            string BirthPlace, string FathersName, string MothersMaidenName, string MothersName, int? Nationality, int Profession, int? Sex,
            DateTime? BirthDate, int? CardType, string FirstName, string LastName, string TCKNo, string SerialNo, string PlaceOfIssue, DateTime? DateOfIssue,
            string[] OtherPhoneNos, string ContactPhoneNo,
            string Culture, /*int? CustomerType, */string Email/*, string[] CorporateCustomerInfo*/, string ReferenceCode, int? TariffId)
        {
            UpdateRandom();
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
                        ReferralDiscountInfo = new ReferralDiscountInfo()
                        {
                            ReferenceNo = ReferenceCode
                        },
                        //BillingPeriod = BillingPeriod,
                        ServiceID = TariffId,//tarifelerden al
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
                    CorporateCustomerInfo = null /*new CorporateCustomerInfo*/
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
            UpdateRandom();
            return InternalClient.PayBills(new NetspeedServicePayBillsRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                PayBillsParameters = billIds
            });
        }

        public NetspeedServiceRegisterCustomerContactResponse RegisterCustomerContact(string FullName, string PhoneNumber)
        {
            UpdateRandom();
            return InternalClient.RegisterCustomerContact(new NetspeedServiceCustomerContactRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                CustomerContactParameters = new CustomerContactRequest
                {
                    FullName = FullName,
                    PhoneNo = PhoneNumber,
                    RequestSubTypeID = 1048,
                    RequestTypeID = 1022,
                }
            });
        }

        public NetspeedServiceSendGenericSMSResponse SendGenericSMS(string phoneNumber)
        {
            UpdateRandom();
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
            UpdateRandom();
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
            UpdateRandom();
            return InternalClient.SubscriberPaymentVPOS(new NetspeedServicePaymentVPOSRequest()
            {
                Username = Username,
                Rand = Rand,
                Hash = CalculateHash(),
                Culture = Culture,
                PaymentVPOSParameters = new PaymentVPOSRequest
                {
                    BillIds = BillList,
                    FailUrl = FailUrl,
                    OkUrl = OkUrl
                }
            });
        }
    }
}