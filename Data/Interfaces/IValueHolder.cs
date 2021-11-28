using System;
using Windows.UI.Xaml;

namespace SchemeCreator.Data.Interfaces
{
    public interface IValueHolder
    {
        public bool? Value { get; set; }
        public Action<bool?> ValueChanged { get; set; }
        public void SwitchValue();
        public void Reset();
    }
    
    public static class ValueHolderExtension
    {
        public static void SwitchControlValue<T>(this T control) where T : FrameworkElement, IValueHolder =>
            control.Value = control.Value == true ? false : control.Value == false ? null : true;

        public static void ResetControlValue<T>(this T control) where T : FrameworkElement, IValueHolder => control.Value = null;
    }
}
