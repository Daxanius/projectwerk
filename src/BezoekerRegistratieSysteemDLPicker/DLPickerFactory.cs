namespace BezoekerRegistratieSysteemDLPicker {
	public static class DLPickerFactory {
		public static BezoekersRegistratieBeheerRepo GeefRepositories(string conString, RepoType repoType) {
			return new BezoekersRegistratieBeheerRepo(conString, repoType);
		}
	}
}
