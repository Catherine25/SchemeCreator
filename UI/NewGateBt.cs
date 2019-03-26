using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchemeCreator.Data {
    class NewGateBt {
    public NewGateBt(SchemeCreator.Constants.GateEnum _type,
        int _inputCount,
        int _outputCount,
        bool _isExternal) {
        
        type = _type;
        inputCount = _inputCount;
        outputCount = _outputCount;
        isExternal = _isExternal;
    }
    public SchemeCreator.Constants.GateEnum type;
    public int inputCount, outputCount;
    public bool isExternal;
    }
}