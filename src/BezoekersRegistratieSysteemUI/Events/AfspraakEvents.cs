﻿using BezoekersRegistratieSysteemUI.Model;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void AfspraakToegevoegdEvent(AfspraakDTO afspraak);
	public delegate void VerwijderAfspraak(AfspraakDTO afspraak);
	public delegate void UpdateAfspraak(AfspraakDTO afspraak);
	public class AfspraakEvents {
		public static event AfspraakToegevoegdEvent NieuweAfspraakToegevoegd;
		public static event VerwijderAfspraak VerwijderAfspraak;
		public static event UpdateAfspraak UpdateAfspraak;

		public static void InvokeNieuweAfspraakToegevoegd(AfspraakDTO afspraak) => NieuweAfspraakToegevoegd?.Invoke(afspraak);
		public static void InvokeVerwijderAfspraak(AfspraakDTO afspraak) => VerwijderAfspraak?.Invoke(afspraak);
		public static void InvokeUpdateAfspraak(AfspraakDTO afspraak) => UpdateAfspraak?.Invoke(afspraak);
	}
}
