﻿#pragma checksum "C:\Users\fran_\source\repos\domopica\domopica\DevicePages\GroundFloorPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2AD13870315FCAA13F2F6BD7FEA2D24F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace domopica.DevicePages
{
    partial class GroundFloorPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // DevicePages\GroundFloorPage.xaml line 11
                {
                    this.scheme = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 3: // DevicePages\GroundFloorPage.xaml line 70
                {
                    this.entrance_light = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.entrance_light).Click += this.LightButton_Click;
                }
                break;
            case 4: // DevicePages\GroundFloorPage.xaml line 51
                {
                    this.garage_door = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.garage_door).Click += this.DoorButton_Click;
                }
                break;
            case 5: // DevicePages\GroundFloorPage.xaml line 57
                {
                    this.garage_light = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.garage_light).Click += this.LightButton_Click;
                }
                break;
            case 6: // DevicePages\GroundFloorPage.xaml line 42
                {
                    this.stairs_light = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.stairs_light).Click += this.LightButton_Click;
                }
                break;
            case 7: // DevicePages\GroundFloorPage.xaml line 29
                {
                    this.kitchen_light = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.kitchen_light).Click += this.LightButton_Click;
                }
                break;
            case 8: // DevicePages\GroundFloorPage.xaml line 14
                {
                    this.living_room_light = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.living_room_light).Click += this.LightButton_Click;
                }
                break;
            case 9: // DevicePages\GroundFloorPage.xaml line 20
                {
                    this.living_room_curtain = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.living_room_curtain).Click += this.CurtainButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
