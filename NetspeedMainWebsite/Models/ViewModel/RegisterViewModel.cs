using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class RegisterViewModel
    {
        public string gsmno { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public int? applicationType { get; set; } // başvuru tipi
        public string hizmetnosu { get; set; }  // xdsl no
        public string mevcutoprad { get; set; } // mevcut operatör adı
        public string evtelno { get; set; } // pstn no
        public int? housephone { get; set; }    // ev tel var mı ?
        public string kapino { get; set; }
        public string katno { get; set; }
        public int? postakodu { get; set; }
        public int? tariff { get; set; }
        public int? modem { get; set; } // hangi modemi istiyor
        public int? statikip { get; set; }  // statik ip
        public string referans { get; set; }
        public int? idCardType { get; set; }
        public string tcno { get; set; }
        public int? dogumgun { get; set; }
        public int? dogumay { get; set; }
        public int? dogumyili { get; set; }
        public string anneadi { get; set; }
        public string babaadi { get; set; }
        public int? cinsiyet { get; set; }
        public string dogumyeri { get; set; }
        public string annekizliksoyad { get; set; }
        public string serino { get; set; }
        public int? kimlikgun { get; set; }
        public int? kimlikay { get; set; }
        public int? kimlikyil { get; set; }

        public string sirano { get; set; }
        public string ciltno { get; set; }
        public string ailesirano { get; set; }
        public string kimlikil { get; set; }
        public string kimlikilce { get; set; }
        public string kimlikmahalle { get; set; }
    }
    public enum IDCardTypes
    {
        TCIDCardWithChip = 1,
        TCBirthCertificate = 2,
        TCForeignerIDCard = 3,
        TCPassportWithChip = 4,
        OldTCPassportDarkBlue = 5,
        OldTCPassportGreen = 6,
        OldTCPassportGrey = 7,
        OldTCPassportRed = 8,
        TCPassportTemporary = 9,
        ForeignerPassport = 10,
        FlightCrewCertificate = 11,
        ShipmansCertificate = 12,
        NATOOrderDocument = 13,
        TravelDocument = 14,
        BorderCrossingDocument = 15,
        ShipCommanderApprovedPersonnelList = 16,
        TCDrivingLisence = 17,
        TCProsecutorJudgeIDCard = 18,
        TCLawyerIDCard = 19,
        TCTemporaryIDCard = 20,
        TCBlueCard = 21,
        TCInternationalFamilyCertificate = 22
    }
}