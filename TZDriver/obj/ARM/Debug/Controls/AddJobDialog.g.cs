﻿#pragma checksum "C:\Users\Chris\Desktop\TZDriver\TZDriver\Controls\AddJobDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4CF70166ED78F966D0166298EE1D761C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TZDriver.Controls
{
    partial class AddJobDialog : 
        global::Windows.UI.Xaml.Controls.ContentDialog, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedItem(global::Windows.UI.Xaml.Controls.Primitives.Selector obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.SelectedItem = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private class AddJobDialog_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IAddJobDialog_Bindings
        {
            private global::TZDriver.Controls.AddJobDialog dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.ComboBox obj7;
            private global::Windows.UI.Xaml.Controls.ComboBox obj8;

            private AddJobDialog_obj1_BindingsTracking bindingsTracking;

            public AddJobDialog_obj1_Bindings()
            {
                this.bindingsTracking = new AddJobDialog_obj1_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 7: // Controls\AddJobDialog.xaml line 113
                        this.obj7 = (global::Windows.UI.Xaml.Controls.ComboBox)target;
                        (this.obj7).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.Primitives.Selector.SelectedItemProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                            if (this.initialized)
                            {
                                // Update Two Way binding
                                this.dataRoot.SelectedJobType = (global::TZDriver.Models.Tools.Utilities.ServiceType)this.obj7.SelectedItem;
                            }
                        });
                        break;
                    case 8: // Controls\AddJobDialog.xaml line 148
                        this.obj8 = (global::Windows.UI.Xaml.Controls.ComboBox)target;
                        (this.obj8).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.Primitives.Selector.SelectedItemProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                            if (this.initialized)
                            {
                                // Update Two Way binding
                                this.dataRoot.SelectedPayMode = (global::TZDriver.Models.Tools.Utilities.TripPayMode)this.obj8.SelectedItem;
                            }
                        });
                        break;
                    default:
                        break;
                }
            }

            // IAddJobDialog_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::TZDriver.Controls.AddJobDialog)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::TZDriver.Controls.AddJobDialog obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_JobTypeList(obj.JobTypeList, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_SelectedJobType(obj.SelectedJobType, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_PayModeList(obj.PayModeList, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_SelectedPayMode(obj.SelectedPayMode, phase);
                    }
                }
            }
            private void Update_JobTypeList(global::System.Collections.Generic.List<global::TZDriver.Models.Tools.Utilities.ServiceType> obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Controls\AddJobDialog.xaml line 113
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj7, obj, null);
                }
            }
            private void Update_SelectedJobType(global::TZDriver.Models.Tools.Utilities.ServiceType obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Controls\AddJobDialog.xaml line 113
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedItem(this.obj7, obj, null);
                }
            }
            private void Update_PayModeList(global::System.Collections.Generic.List<global::TZDriver.Models.Tools.Utilities.TripPayMode> obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Controls\AddJobDialog.xaml line 148
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj8, obj, null);
                }
            }
            private void Update_SelectedPayMode(global::TZDriver.Models.Tools.Utilities.TripPayMode obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Controls\AddJobDialog.xaml line 148
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedItem(this.obj8, obj, null);
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
            private class AddJobDialog_obj1_BindingsTracking
            {
                private global::System.WeakReference<AddJobDialog_obj1_Bindings> weakRefToBindingObj; 

                public AddJobDialog_obj1_BindingsTracking(AddJobDialog_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<AddJobDialog_obj1_Bindings>(obj);
                }

                public AddJobDialog_obj1_Bindings TryGetBindingObject()
                {
                    AddJobDialog_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_(null);
                }

                public void PropertyChanged_(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    AddJobDialog_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::TZDriver.Controls.AddJobDialog obj = sender as global::TZDriver.Controls.AddJobDialog;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_SelectedJobType(obj.SelectedJobType, DATA_CHANGED);
                                bindings.Update_SelectedPayMode(obj.SelectedPayMode, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "SelectedJobType":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SelectedJobType(obj.SelectedJobType, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "SelectedPayMode":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SelectedPayMode(obj.SelectedPayMode, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                public void UpdateChildListeners_(global::TZDriver.Controls.AddJobDialog obj)
                {
                    AddJobDialog_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        if (bindings.dataRoot != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)bindings.dataRoot).PropertyChanged -= PropertyChanged_;
                        }
                        if (obj != null)
                        {
                            bindings.dataRoot = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Controls\AddJobDialog.xaml line 24
                {
                    global::Windows.UI.Xaml.Controls.StackPanel element2 = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    ((global::Windows.UI.Xaml.Controls.StackPanel)element2).Tapped += this.selectContact_Tapped;
                }
                break;
            case 3: // Controls\AddJobDialog.xaml line 57
                {
                    this.relativeJobGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4: // Controls\AddJobDialog.xaml line 70
                {
                    this.ClientName = (global::TZDriver.Models.Controls.TextboxControl)(target);
                }
                break;
            case 5: // Controls\AddJobDialog.xaml line 83
                {
                    this.PhoneNumber = (global::TZDriver.Models.Controls.TextboxControl)(target);
                }
                break;
            case 6: // Controls\AddJobDialog.xaml line 95
                {
                    this.PickTime = (global::Windows.UI.Xaml.Controls.TimePicker)(target);
                }
                break;
            case 7: // Controls\AddJobDialog.xaml line 113
                {
                    this.JobType = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 8: // Controls\AddJobDialog.xaml line 148
                {
                    this.PaymentOption = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 9: // Controls\AddJobDialog.xaml line 183
                {
                    this.saveClient = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.saveClient).Click += this.saveClient_Click;
                }
                break;
            case 10: // Controls\AddJobDialog.xaml line 197
                {
                    this.cancelOperation = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.cancelOperation).Click += this.cancelOperation_Click;
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
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // Controls\AddJobDialog.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.ContentDialog element1 = (global::Windows.UI.Xaml.Controls.ContentDialog)target;
                    AddJobDialog_obj1_Bindings bindings = new AddJobDialog_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

