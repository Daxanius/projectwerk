using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL {
    public class AfspraakRepoADO : IAfspraakRepository {
        private string _connectieString;

        public AfspraakRepoADO(string connectieString) {
            _connectieString = connectieString;
        }
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }
        public void BeeindigAfspraak(uint afspraakId) {
            throw new NotImplementedException();
        }

        public void BewerkAfspraak(Afspraak afspraak) {
            throw new NotImplementedException();
        }

        public Afspraak GeefAfspraak(uint afspraakid) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId) {
            throw new NotImplementedException();
        }

        public void VerwijderAfspraak(uint afspraakId) {
            throw new NotImplementedException();
        }

        public void VoegAfspraakToe(Afspraak afspraak) {
            throw new NotImplementedException();
        }
    }
}
