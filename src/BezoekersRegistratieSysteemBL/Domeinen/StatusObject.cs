using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class StatusObject {
		public string Statusnaam { get; private set; }
		public object Obj { get; private set; }

		public StatusObject(string statusnaam, object obj) {
			ZetStatusNaam(statusnaam);
			ZetObject(obj);
		}
		public void ZetObject(object obj) {
			if (obj is null) {
				throw new StatusObjectException("Object is null");
			}
			Obj = obj;
		}
		public void ZetStatusNaam(string statusNaam) {
			if (String.IsNullOrWhiteSpace(statusNaam)) {
				throw new StatusObjectException("statusnaam is leeg");
			}
			Statusnaam = statusNaam.Trim();
		}
		public Werknemer GeefWerknemerObject() {
			if (Obj is Werknemer werknemer) {
				return werknemer;
			} else {
				throw new StatusObjectException("Object is geen werknemer");
			}
		}

		public Bedrijf GeefBedrijfObject() {
			if (Obj is Bedrijf bedrijf) {
				return bedrijf;
			} else {
				throw new StatusObjectException("Object is geen bedrijf");
			}
		}

		public Afspraak GeefAfspraakObject() {
			if (Obj is Afspraak afspraak) {
				return afspraak;
			} else {
				throw new StatusObjectException("Object is geen afspraak");
			}
		}
	}
}
