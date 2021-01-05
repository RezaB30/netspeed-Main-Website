using NetspeedMainWebsite.MainSiteServiceReference;
using NetspeedMainWebsite.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Controllers
{
    public class ApplicationController : Controller
    {
        MainSiteServiceClient client = new MainSiteServiceClient();
        // GET: Application
        //public ActionResult Index()
        //{
        //    return View();
        //}




        //    [HttpPost]
        //    public ActionResult Index(ApplicationViewModel application)
        //    {

        //        var randomKey = Guid.NewGuid().ToString();
        //        var username = "elif";
        //        var passwordHash = HashUtilities.HashCalculate("123456");
        //        var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


        //        var response = client.NewCustomerRegister(new NetspeedServiceNewCustomerRegisterRequest()
        //        {

        //            Culture = "tr-tr",
        //            Rand = randomKey,
        //            Hash = serviceRequestHash,
        //            Username = username,
        //            CustomerRegisterParameters = new NewCustomerRegisterRequest()
        //            {
        //                SubscriptionInfo = new SubscriptionRegistrationInfo()
        //                {
        //                    BillingPeriod = "",//fatura dönemi
        //                    DomainID = "",//büyük ihtimal sabit
        //                    ServiceID = "",
        //                    SetupAddress = new AddressInfo()
        //                    {

        //                        ProvinceID =,
        //                        ProvinceName = application.Province,
        //                        DistrictID = application.DistrictId,
        //                        DistrictName = application.District,
        //                        NeighbourhoodID = application.NeighborhoodId,
        //                        NeighbourhoodName = application.Neighborhood,
        //                        StreetID = application.StreetId,
        //                        StreetName = application.Street,
        //                        ApartmentID = application.AparmentId,
        //                        ApartmentNo = application.AparmentNo,
        //                        DoorID = application.DoorId,
        //                        DoorNo = application.DoorNumber,
        //                        AddressNo =,
        //                        AddressText =,
        //                        Floor =,
        //                        PostalCode = application.PostalCode,
        //                        RuralCode =
        //                        }
        //                },
        //                IndividualCustomerInfo = new IndividualCustomerInfo()
        //                {
        //                    BirthPlace = application.BirthPlace,
        //                    FathersName = application.FatherName,
        //                    MothersMaidenName = application.MotherFirstSurname,
        //                    MothersName = application.MotherName,
        //                    Nationality = "",
        //                    Profession = "",
        //                    ResidencyAddress =new AddressInfo()
        //                    {
        //                        ProvinceID =application.ProvinceId,
        //                        ProvinceName = application.Province,
        //                        DistrictID =application.DistrictId,
        //                        DistrictName = application.District,
        //                        NeighbourhoodID =application.NeighborhoodId,
        //                        NeighbourhoodName =application.Neighborhood,
        //                        StreetID =application.StreetId,
        //                        StreetName =application.Street,
        //                        ApartmentID =application.AparmentId,
        //                        ApartmentNo =application.AparmentNo,
        //                        DoorID = application.DoorId,
        //                        DoorNo = application.DoorNumber,
        //                        AddressNo =,
        //                        AddressText=,
        //                        Floor=,
        //                        PostalCode = application.PostalCode,
        //                        RuralCode =
        //                    },
        //                    Sex = application.Sex
        //                },
        //                IDCardInfo = new IDCardInfo()
        //                {
        //                    BirthDate="",
        //                    CardType = "",
        //                    DateOfIssue = "",
        //                    District = "",
        //                    FirstName=application.FirstName,
        //                    LastName=application.LastName,
        //                    Neighbourhood = "",
        //                    PageNo = "",
        //                    PassportNo = "",
        //                    PlaceOfIssue = "",
        //                    Province = "",
        //                    RowNo = "",
        //                    SerialNo = "",
        //                    TCKNo = "",
        //                    VolumeNo = ""
        //                },
        //                CustomerGeneralInfo = new CustomerGeneralInfo()
        //                {
        //                    BillingAddress = new AddressInfo()
        //                    {
        //                        ProvinceID = application.ProvinceId,
        //                        ProvinceName = application.Province,
        //                        DistrictID = application.DistrictId,
        //                        DistrictName = application.District,
        //                        NeighbourhoodID = application.NeighborhoodId,
        //                        NeighbourhoodName = application.Neighborhood,
        //                        StreetID = application.StreetId,
        //                        StreetName = application.Street,
        //                        ApartmentID = application.AparmentId,
        //                        ApartmentNo = application.AparmentNo,
        //                        DoorID = application.DoorId,
        //                        DoorNo = application.DoorNumber,
        //                        AddressNo =,
        //                        AddressText =,
        //                        Floor =,
        //                        PostalCode = application.PostalCode,
        //                        RuralCode =
        //                        },
        //                    ContactPhoneNo = "",
        //                    Culture = "",
        //                    CustomerType = "",
        //                    Email=application.EmailAddress,
        //                    OtherPhoneNos = new PhoneNoListItem()
        //                    { 
        //                        Number=,
        //                    }
        //                },
        //                CorporateCustomerInfo = new CorporateCustomerInfo()
        //                {
        //                    CentralSystemNo = "",
        //                    ExecutiveMothersMaidenName =,
        //                    ExecutiveMothersName =,
        //                    ExecutiveNationality =,
        //                    ExecutiveProfession =,
        //                    ExecutiveResidencyAddress =new AddressInfo()
        //                    {
        //                        ProvinceID = application.ProvinceId,
        //                        ProvinceName = application.Province,
        //                        DistrictID = application.DistrictId,
        //                        DistrictName = application.District,
        //                        NeighbourhoodID = application.NeighborhoodId,
        //                        NeighbourhoodName = application.Neighborhood,
        //                        StreetID = application.StreetId,
        //                        StreetName = application.Street,
        //                        ApartmentID = application.AparmentId,
        //                        ApartmentNo = application.AparmentNo,
        //                        DoorID = application.DoorId,
        //                        DoorNo = application.DoorNumber,
        //                        AddressNo =,
        //                        AddressText =,
        //                        Floor =,
        //                        PostalCode = application.PostalCode,
        //                        RuralCode =
        //                    },
        //                    ExecutiveSex =,
        //                    TaxNo =,
        //                    TaxOffice =,
        //                    Title =,
        //                    TradeRegistrationNo =,
        //                    CompanyAddress =new AddressInfo()
        //                    {
        //                        ProvinceID = application.ProvinceId,
        //                        ProvinceName = application.Province,
        //                        DistrictID = application.DistrictId,
        //                        DistrictName = application.District,
        //                        NeighbourhoodID = application.NeighborhoodId,
        //                        NeighbourhoodName = application.Neighborhood,
        //                        StreetID = application.StreetId,
        //                        StreetName = application.Street,
        //                        ApartmentID = application.AparmentId,
        //                        ApartmentNo = application.AparmentNo,
        //                        DoorID = application.DoorId,
        //                        DoorNo = application.DoorNumber,
        //                        AddressNo =,
        //                        AddressText =,
        //                        Floor =,
        //                        PostalCode = application.PostalCode,
        //                        RuralCode =

        //                    },
        //                    ExecutiveBirthPlace =,
        //                    ExecutiveFathersName =,
        //                },
        //            }

        //        });
        //        var k = response.NewCustomerRegisterResponse.Select(r => new 
        //        { 

        //        });


        //        return View();
        //    }


        public ActionResult Index()
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var genericHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetProvinces(new NetspeedServiceRequests()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = genericHash,
                Username = username,
            });

            var ProvinceItems = response.ValueNamePairList.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });

            return View(new ApplicationViewModel() { ProvinceList = ProvinceItems });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDistricts(long code)
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


            var response = client.GetProvinceDistricts(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });

            var DistrictItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (DistrictItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(DistrictItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRegions(long code)
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


            var response = client.GetDistrictRuralRegions(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });

            var RegionItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (RegionItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(RegionItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetNeighborhoods(long code)
        {

            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


            var response = client.GetRuralRegionNeighbourhoods(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });
            var NeighborhoodsItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (NeighborhoodsItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(NeighborhoodsItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetStreets(long code)
        {

            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetNeighbourhoodStreets(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });

            var StreetItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (StreetItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(StreetItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetBuildings(long code)
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetStreetBuildings(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });

            var BuildingItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (BuildingItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(BuildingItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetApartments(long code)
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetBuildingApartments(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });
            var BuildingItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (BuildingItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(BuildingItems, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult _ContactInformation(ApplicationViewModel application)
        //{
        //    var contactInformationList = new List<ApplicationViewModel>();


        //    contactInformationList.Add(new ApplicationViewModel()
        //    {
        //        FirstName = application.FirstName,
        //        LastName = application.LastName,
        //        EmailAddress = application.EmailAddress
        //    });
        //    Session["contactInformationList"] = contactInformationList;

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult _AddressAndApplicationInformation(ApplicationViewModel application)
        //{
        //    var addressAndApplicationInformationList = new List<ApplicationViewModel>();

        //    addressAndApplicationInformationList.Add(new ApplicationViewModel()
        //    {
        //        ApplicationType=application.ApplicationType,
        //        HasFixedPhone=application.HasFixedPhone,
        //        FixedPhoneNumber=application.FixedPhoneNumber,
        //        ServiceNumber=application.ServiceNumber,
        //        OperatorName=application.OperatorName,
        //        ProvinceId = application.ProvinceId,
        //        Province = application.Province,
        //        DistrictId = application.DistrictId,
        //        District = application.District,
        //        RegionId = application.RegionId,
        //        Region = application.Region,
        //        NeighborhoodId = application.NeighborhoodId,
        //        Neighborhood = application.Neighborhood,
        //        StreetId = application.StreetId,
        //        Street = application.Street,
        //        Floor = application.Floor,
        //        BuildingId=application.BuildingId,
        //        Building=application.Building,
        //        ApartmentId=application.ApartmentId,
        //        Apartment=application.Apartment,
        //        PostalCode = application.PostalCode,
        //    });
        //    Session["addressAndApplicationInformationList"] = addressAndApplicationInformationList;
        //    return View();
        //}

        //public ActionResult _TariffsAndPreference(ApplicationViewModel application)
        //{
        //    var tariffsAndPreferencenList = new List<ApplicationViewModel>();


        //    tariffsAndPreferencenList.Add(new ApplicationViewModel()
        //    {

        //    });
        //    Session["tariffsAndPreferencenList"] = tariffsAndPreferencenList;

        //    return View();

        //}
        
        [HttpPost]
        public ActionResult Index(ApplicationViewModel application)
        {
            if (ModelState.IsValid)
            {

                var randomKey = Guid.NewGuid().ToString();
                var username = "elif";
                var passwordHash = HashUtilities.HashCalculate("123456");
                var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


                var response = client.NewCustomerRegister(new NetspeedServiceNewCustomerRegisterRequest()
                {

                    Culture = "tr-tr",
                    Rand = randomKey,
                    Hash = serviceRequestHash,
                    Username = username,
                    CustomerRegisterParameters = new NewCustomerRegisterRequest()
                    {
                        SubscriptionInfo = new SubscriptionRegistrationInfo()
                        {
                            BillingPeriod = application.BillingPeriod,
                            DomainID = application.DomainId,
                            ServiceID = application.ServiceId,
                            SetupAddress = new AddressInfo()
                            {

                                ProvinceID = application.ProvinceId,
                                //ProvinceName = application.Province,
                                DistrictID = application.DistrictId,
                                //DistrictName = application.District,
                                NeighbourhoodID = application.NeighborhoodId,
                                //NeighbourhoodName = application.Neighborhood,
                                StreetID = application.StreetId,
                                //StreetName = application.Street,
                                ApartmentID = application.BuildingId,
                                //ApartmentNo = application.Building,
                                DoorID = application.ApartmentId,
                                //DoorNo = application.Apartment,
                                AddressNo = application.AddressNo,
                                AddressText = application.AddressText,
                                Floor = application.Floor,
                                PostalCode = application.PostalCode,
                                RuralCode = application.RuralCode
                            }
                        },
                        IndividualCustomerInfo = new IndividualCustomerInfo()
                        {
                            BirthPlace = application.BirthPlace,
                            FathersName = application.FatherName,
                            MothersMaidenName = application.MotherFirstSurname,
                            MothersName = application.MotherName,
                            Nationality = application.Nationality,
                            Profession = application.Profession,
                            Sex = application.Sex,
                            ResidencyAddress = new AddressInfo()
                            {
                                ProvinceID = application.ProvinceId,
                                //ProvinceName = application.Province,
                                DistrictID = application.DistrictId,
                                //DistrictName = application.District,
                                NeighbourhoodID = application.NeighborhoodId,
                                //NeighbourhoodName = application.Neighborhood,
                                StreetID = application.StreetId,
                                //StreetName = application.Street,
                                ApartmentID = application.BuildingId,
                                //ApartmentNo = application.Building,
                                DoorID = application.ApartmentId,
                                //DoorNo = application.Apartment,
                                AddressNo = application.AddressNo,
                                AddressText = application.AddressText,
                                Floor = application.Floor,
                                PostalCode = application.PostalCode,
                                RuralCode = application.RuralCode
                            }

                        },
                        IDCardInfo = new IDCardInfo()
                        {
                            BirthDate = application.BirthDay,
                            CardType = application.CardType,
                            DateOfIssue = application.DateOfIssue,
                            District = application.District,
                            FirstName = application.FirstName,
                            LastName = application.LastName,
                            Neighbourhood = application.Neighborhood,
                            PageNo = application.PageNo,
                            PassportNo = application.PassportNo,
                            PlaceOfIssue = application.PlaceOfIssue,
                            //Province = application.Province,
                            RowNo = application.RowNo,
                            SerialNo = application.SerialNo,
                            TCKNo = application.TC,
                            VolumeNo = application.VolumeNo
                        },
                        CustomerGeneralInfo = new CustomerGeneralInfo()
                        {
                            BillingAddress = new AddressInfo()
                            {
                                ProvinceID = application.ProvinceId,
                                //ProvinceName = application.Province,
                                DistrictID = application.DistrictId,
                                //DistrictName = application.District,
                                NeighbourhoodID = application.NeighborhoodId,
                                //NeighbourhoodName = application.Neighborhood,
                                StreetID = application.StreetId,
                                //StreetName = application.Street,
                                ApartmentID = application.BuildingId,
                                //ApartmentNo = application.Building,
                                DoorID = application.ApartmentId,
                                //DoorNo = application.Apartment,
                                AddressNo = application.AddressNo,
                                AddressText = application.AddressText,
                                Floor = application.Floor,
                                PostalCode = application.PostalCode,
                                RuralCode = application.RuralCode
                            },
                            ContactPhoneNo = application.PhoneNumber,
                            Culture = application.Culture,
                            CustomerType = application.CustomerType,
                            Email = application.EmailAddress,
                            //OtherPhoneNos=new PhoneNoListItem()
                            //{
                            //    //Number=,
                            //}
                        },
                        CorporateCustomerInfo = new CorporateCustomerInfo()
                        {
                            CentralSystemNo = application.CentralSystemNo,
                            ExecutiveMothersMaidenName = application.MotherFirstSurname,
                            ExecutiveMothersName = application.MotherName,
                            ExecutiveNationality = application.Nationality,
                            ExecutiveProfession = application.Profession,
                            ExecutiveResidencyAddress = new AddressInfo()
                            {
                                ProvinceID = application.ProvinceId,
                                //ProvinceName = application.Province,
                                DistrictID = application.DistrictId,
                                //DistrictName = application.District,
                                NeighbourhoodID = application.NeighborhoodId,
                                //NeighbourhoodName = application.Neighborhood,
                                StreetID = application.StreetId,
                                //StreetName = application.Street,
                                ApartmentID = application.BuildingId,
                                //ApartmentNo = application.Building,
                                DoorID = application.ApartmentId,
                                //DoorNo = application.Apartment,
                                AddressNo = application.AddressNo,
                                AddressText = application.AddressText,
                                Floor = application.Floor,
                                PostalCode = application.PostalCode,
                                RuralCode = application.RuralCode
                            },
                            ExecutiveSex = application.Sex,
                            TaxNo = application.TaxNo,
                            TaxOffice = application.TaxOffice,
                            Title = application.Title,
                            TradeRegistrationNo = application.TradeRegistrationNo,
                            CompanyAddress = new AddressInfo()
                            {
                                ProvinceID = application.ProvinceId,
                                //ProvinceName = application.Province,
                                DistrictID = application.DistrictId,
                                //DistrictName = application.District,
                                NeighbourhoodID = application.NeighborhoodId,
                                //NeighbourhoodName = application.Neighborhood,
                                StreetID = application.StreetId,
                                //StreetName = application.Street,
                                ApartmentID = application.BuildingId,
                                //ApartmentNo = application.Building,
                                DoorID = application.ApartmentId,
                                //DoorNo = application.Apartment,
                                AddressNo = application.AddressNo,
                                AddressText = application.AddressText,
                                Floor = application.Floor,
                                PostalCode = application.PostalCode,
                                RuralCode = application.RuralCode

                            },
                            ExecutiveBirthPlace = application.BirthPlace,
                            ExecutiveFathersName = application.FatherName,
                        },
                    }

                });

                //var k = response.NewCustomerRegisterResponse.Select(r => new ApplicationViewModel()
                //{

                //});

                var message = response.ResponseMessage.ErrorMessage;
                return View();

            }
            return View();
        }

    }
}