using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL {
    public class BedrijfRepoADO : IBedrijfRepository {
        private string _connectieString;

        public BedrijfRepoADO(string connectieString) {
            _connectieString = connectieString;
        }
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }

        public bool BestaatBedrijf(Bedrijf bedrijf) {
            throw new NotImplementedException();
        }

        public bool BestaatBedrijf(uint bedrijf) {
            throw new NotImplementedException();
        }

        public bool BestaatBedrijf(string bedrijfsnaam) {
            throw new NotImplementedException();
        }

        public void BewerkBedrijf(Bedrijf bedrijf) {
            throw new NotImplementedException();
        }

        public Bedrijf GeefBedrijf(uint id) {
            throw new NotImplementedException();
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bedrijf> Geefbedrijven() {
            throw new NotImplementedException();
        }

        public void VerwijderBedrijf(uint id) {
            throw new NotImplementedException();
        }

        public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
            throw new NotImplementedException();
        }
    }
}
