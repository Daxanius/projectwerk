using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekerRegistratieSysteemDLPicker {
    public static class DLPickerFactory {
        public static BezoekersRegistratieBeheerRepo GeefRepositories(string conString, RepoType repoType) {
            return new BezoekersRegistratieBeheerRepo(conString, repoType);
        }
    }
}
