namespace BezoekersRegistratieSysteemUI.Api.Input
{
	public class AfspraakInputDTO
	{
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		public AfspraakInputDTO(BezoekerInputDTO bezoeker, long werknemerId, long bedrijfId)
		{
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
			BedrijfId = bedrijfId;
		}

		/// <summary>
		/// De bezoeker van de afspraak.
		/// </summary>
		public BezoekerInputDTO Bezoeker { get; set; }

		/// <summary>
		/// De werknemer waarmee de afspraak is gemaakt.
		/// </summary>
		public long WerknemerId { get; set; }

		/// <summary>
		/// Het bedrijf waarbinnen de afspraak is gemaakt.
		/// </summary>
		public long BedrijfId { get; set; }
	}
}