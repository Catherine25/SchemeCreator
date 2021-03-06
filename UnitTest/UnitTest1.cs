using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateExternalGateView()
        {
            //GateView gateView = new GateView(GateEnum.IN, new Vector2(0, 0));
            //Assert.AreEqual(0, gateView.InputCount);
            //Assert.AreEqual(0, gateView.OutputCount);
            //Assert.AreEqual(GateEnum.IN, gateView.Type);
            //Assert.AreEqual(new Vector2(0, 0), gateView.MatrixIndex);
            //Assert.IsNull(gateView.Values);
        }

        [TestMethod]
        public void CreateExternalGateViewWithWrongInputCount()
        {
            try
            {
                //GateView gateView = new GateView(GateEnum.IN, new Vector2(0, 0), 2, 2);
            }
            catch
            {

            }
            //Assert.ThrowsException<GateValidationError>(() => new GateView(GateEnum.IN, new Vector2(0, 0), 2, 2));
        }

        [TestMethod]
        public void CreateLogicalGateView()
        {
            //GateView gateView = new GateView(GateEnum.IN, new Vector2(0, 0));
            //Assert.AreEqual(0, gateView.InputCount);
            //Assert.AreEqual(0, gateView.OutputCount);

            //GateView gateView = new GateView(GateEnum.AND, new Vector2(0, 0));
            //Assert.IsNotNull(gateView.InputCount);
            //Assert.IsNotNull(gateView.OutputCount);
        }
    }
}
