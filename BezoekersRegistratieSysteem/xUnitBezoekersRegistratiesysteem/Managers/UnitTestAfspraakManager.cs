using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class AfspraakManagerTest
	{
        private AfspraakManager _afspraakManager;
        private Mock<IAfspraakRepository> _mockRepo;

        Werknemer _w = new();

        //[Fact]
        //public void VoegAfspraakToe_Invalid()
        //{
        //}
    }
}