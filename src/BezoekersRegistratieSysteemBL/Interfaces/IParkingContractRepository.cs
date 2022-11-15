using BezoekersRegistratieSysteemBL.Domeinen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
    public interface IParkingContractRepository
    {
        bool BestaatParkingContract(ParkingContract parkingContract);
        void BewerkParkingContract(ParkingContract parkingContract);
        ParkingContract GeefParkingContract(long id);
        void VerwijderParkingContract(ParkingContract parkingContract);
        void VoegParkingContractToe(ParkingContract parkingContract);
    }
}
