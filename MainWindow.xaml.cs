using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Security.Policy;
using static System.Windows.Forms.LinkLabel;
using System.Windows.Markup;
using System.Timers;
using System.Media;
using NAudio.Wave;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Data;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {


        public string SelectedTags { get; set; }




        private void UpdateRTB()
        {
            // Create the animation
            DoubleAnimation fadeIn = new DoubleAnimation();
            fadeIn.From = 0;
            fadeIn.To = 1;
            fadeIn.Duration = new Duration(TimeSpan.FromSeconds(1));

            // Get the current values of the input fields
            string typeOfAccident = ((ComboBoxItem)TypeOfAccident.SelectedItem)?.Content?.ToString();
            string dateOfIncident = DateOfIncident.Text;
            string cityOfIncident = CityofIncident.Text;
            string stateOfIncident = ((ComboBoxItem)StateOfIncident.SelectedItem)?.Content?.ToString();
            string publicTransportSystem = PublicTransportSystem.Text;

            string initialInfo = "";


            if (ApproxDateOfIncident.IsChecked == true)
            {
                initialInfo += "*The date of the incident is approximate\n\n";
            }

            if (ApproxLawyerSignDate.IsChecked == true)
            {
                initialInfo += "*The date IP signed with their lawyer is approximate\n\n";
            }

            if (PCforIP.IsChecked == true)
            {
                initialInfo += "PC is calling on behalf of IP, PCs " + IPrelationtoPC.Text + ". ";
            }
            else if (CforPC.IsChecked == true)
            {
                initialInfo += "Caller is calling on behalf of PC, the callers " + PCrelationtoC.Text + ". ";
            }
            else if (CforIP.IsChecked == true)
            {
                initialInfo += "Caller is calling on behalf of IP, ";

                if (IPrelationtoC.Text != null)
                {

                    initialInfo += "the callers " + IPrelationtoC.Text + ". ";

                }


                if (PCrelationtoIP.Text != null)
                {

                    initialInfo += "PC is IPs " + PCrelationtoIP.Text + ". ";

                }
            }

            string CarService = "";



            if (DriverinCarService.IsChecked == true)
            {
                CarService = "IP is a driver for " + IPsCarServiceCompany.Text + ".";
            }


            string ActualDOI = "";

            if (PCforIP.IsChecked == true)
            {
                ActualDOI = ActualDOI.Replace("PC", "IP");
            }

            if (CforIP.IsChecked == true)
            {
                ActualDOI = ActualDOI.Replace("PC", "IP");
            }

            if (CforPC.IsChecked == true)
            {
                ActualDOI = ActualDOI.Replace("IP", "PC");
            }

            if (!string.IsNullOrEmpty(typeOfAccident))
            {
                ActualDOI += "IP was involved in a " + typeOfAccident + " accident";
            }

            if (!string.IsNullOrEmpty(dateOfIncident))
            {
                if (ApproxDateOfIncident.IsChecked == true)
                {
                    ActualDOI += " around " + dateOfIncident;
                }
                else
                {
                    ActualDOI += " on " + dateOfIncident;
                }
            }

            if (!string.IsNullOrEmpty(cityOfIncident))
            {
                ActualDOI += " in " + cityOfIncident;

            }

            if (!string.IsNullOrEmpty(stateOfIncident))
            {
                ActualDOI += ", " + stateOfIncident + ". ";
            }

            if (IPWorkingCheckBox.IsChecked == true)
            {
                ActualDOI += "IP was on the clock, working at the time of the accident. ";

                if (IPJobComboBox.Text == "Other")
                {
                    ActualDOI += "IP works as a " + IPOtherJobTextBox.Text + " for their company, " + IPCompanyTextBox.Text + ". ";
                }
                else
                {
                    ActualDOI += "IP works as a " + IPJobComboBox.Text + " for their company, " + IPCompanyTextBox.Text + ". ";
                }
            }

            if (TeslaCheckBox.IsChecked == true)
            {
                ActualDOI += "IP was in a Tesla. ";
            }

            if (CommercialVehicleCheckBox.IsChecked == true)
            {
                ActualDOI += "OP was driving a " + CommercialVehicleComboBox.Text + ". ";
            }


            if (IPWasDriver.IsChecked == true && CarServiceCheckBox.IsChecked == true && DriverFUQuestion1Yes.IsChecked == true && DriverinCarService.IsChecked == true)
            {
                ActualDOI += "IP is a driver for " + IPsCarServiceCompany.Text + ", IP had a passenger in the car with him at the time of the accident as well. ";
            }
            else
            {
                if (IPWasDriver.IsChecked == true)
                {
                    ActualDOI += "IP was driving. ";

                    if (DriverFUQuestion1No.IsChecked == true)
                    {
                        ActualDOI += "There were no other passengers in the car. ";
                    }

                    if (DriverFUQuestion1Yes.IsChecked == true)
                    {
                        string relationText = IPDriverPassengerRelations.Text;
                        string verb = "were";
                        string[] words = relationText.Split(' ');
                        if (words.Length == 1)
                        {
                            if (!relationText.EndsWith("s"))
                            {
                                verb = "was";
                            }
                        }
                        else if (relationText.EndsWith("s"))
                        {
                            verb = "were";
                        }
                        ActualDOI += "IP's " + relationText + " " + verb + " also in the car. ";
                    }
                }
            }


            if (IPWasMassTransitPassenger.IsChecked == true)
            {
                ActualDOI += "IP was a mass transit passenger in a " + publicTransportSystem + ". ";

            }

            if (IPWasPassenger.IsChecked == true)
            {
                ActualDOI += "IP was a passenger. The driver of the car was IPs " + IPPassengerDriverRelations.Text + ". ";

                if (IPPassengerPassengersOtherThanDriver.IsChecked == true)
                {
                    string relationText = IPPassengerOtherPassengerRelations.Text;
                    string verb = "were";
                    string[] words = relationText.Split(' ');

                    if (words.Length == 1)
                    {
                        if (!relationText.EndsWith("s"))
                        {
                            verb = "was";
                        }
                    }
                    else if (relationText.EndsWith("s"))
                    {
                        verb = "were";
                    }

                    if (relationText == IPPassengerDriverRelations.Text)
                    {
                        relationText = "other " + relationText;
                    }

                    ActualDOI += "IPs " + relationText + " " + verb + " also in the car. ";
                }
            }


            if (IPWasPedestrian.IsChecked == true)
            {
                ActualDOI += "IP was a pedestrian. ";

            }

            if (IPWasCyclist.IsChecked == true)
            {
                ActualDOI += "IP was a cyclist. ";

            }


            if (AccidentDetailsTextBox.Text != "")
            {
                ActualDOI += AccidentDetailsTextBox.Text + " ";
            }

            if (AirbagsCheckBox.IsChecked == true)
            {
                ActualDOI += "IPs airbags deployed. ";
            }


            if (CarExplodedCheckBox.IsChecked == true && TireExplodedCheckBox.IsChecked == true)
            {
                ActualDOI += "IP's tire and car exploded due to the accident. ";
            }
            else if (CarExplodedCheckBox.IsChecked == true)
            {
                ActualDOI += "IP's car exploded due to the accident. ";
            }
            else if (TireExplodedCheckBox.IsChecked == true)
            {
                ActualDOI += "IP's tire exploded due to the accident. ";
            }


            if (policeNo.IsChecked == true)
            {
                ActualDOI += "The police did not come to the scene of the accident. ";

            }

            if (policeYes.IsChecked == true)
            {
                ActualDOI += "Police came to the scene of the accident. ";

            }

            if (Police_ReportYes.IsChecked == true)
            {
                ActualDOI += "A police report was filed. ";

                if (HavePolice_CopyYes.IsChecked == true)
                {
                    ActualDOI += "IP also has a copy of the police report. ";
                }
                else if (HavePolice_CopyNo.IsChecked == true)
                {
                    ActualDOI += "IP does not have a copy of the police report. ";
                }
            }



            if (Police_ReportNo.IsChecked == true)
            {
                ActualDOI += "IP claims a police report was not filed. ";

            }

            if (Police_ReportIDK.IsChecked == true)
            {
                ActualDOI += "IP is not sure if a police report was filed. ";

            }


            if (HavePolice_ReportNo.IsChecked == true)
            {
                ActualDOI += "IP does not have the police report number at the moment. ";

            }

            if (policeFaultIDK.IsChecked == true)

            {
                ActualDOI += "IP does not know who was found at fault. ";

            }

            if (policeFaultOP.IsChecked == true)

            {
                ActualDOI += "OP was found at fault. ";

            }


            if (DUIOtherDriverCheckBox.IsChecked == true)
            {
                ActualDOI += "OP was cited for driving under the influence. ";
            }

            if (policeFaultPC.IsChecked == true)

            {
                ActualDOI += "IP was found at fault but does not believe they are. ";
                 
            }

            if (policeFaultPCadmits.IsChecked == true)

            {
                ActualDOI += "IP was found at fault and admits they are at fault in the accident. ";

            }


            if (PoliceReportNumberTextBox.Text != "")
            {
                ActualDOI += "\nPR#: " + PoliceReportNumberTextBox.Text;

            }

            ActualDOI += "\n\n" + sInjuryDetails;

            ActualDOI += "\n\n" + FinalTreatmentDetails;


            if (YesAttorneyButton.IsChecked == true)
            {
                ActualDOI += "\n\nIP was previously represented by " + LawyerNameTextBox.Text;

                if (!string.IsNullOrEmpty(LawyerSignDatePicker.Text))
                {

                    ActualDOI += " IP signed with them";

                    if (ApproxLawyerSignDate.IsChecked == true)
                    {
                        ActualDOI += " around " + LawyerSignDatePicker.Text;
                    }
                    else
                    {
                        ActualDOI += " on " + LawyerSignDatePicker.Text;
                    }
                }

                ActualDOI += ". " + CaseProgressTextBox.Text;
            }

            if (!string.IsNullOrEmpty(SettlementOfferFromTextBox.Text))
            {
                if (string.IsNullOrEmpty(SettlementOfferAmountTextBox.Text))
                {
                    ActualDOI += "\n\nIP accepted a settlement offer from " + SettlementOfferFromTextBox.Text + ".";
                }
                else
                {
                    ActualDOI += " IP accepted a settlement offer of " + SettlementOfferAmountTextBox.Text + " from " + SettlementOfferFromTextBox.Text + ".";
                }
            }

            string AdditionalInfo = "\n\n" + OtherInformation.Text;

            if (!string.IsNullOrEmpty(OtherInformation.Text))
            {
                ActualDOI += AdditionalInfo;
            }

            ActualDOI += sInjuryBullets;

            ActualDOI += sTreatmentBullets;

            string FullDOI = initialInfo + ActualDOI;

            if (String.IsNullOrWhiteSpace(FullDOI))
            {
                FullDOI = "DOI";
            }


            tbDisplay.Document.Blocks.Clear();
            tbDisplay.Document.Blocks.Add(new Paragraph(new Run(FullDOI)));

            tbDisplay.BeginAnimation(TextBlock.OpacityProperty, fadeIn);


        }



        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(tbDisplay.Document.ContentStart, tbDisplay.Document.ContentEnd);
            System.Windows.Clipboard.SetData(System.Windows.DataFormats.Text, textRange.Text);
        }


        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateRTB();
            }
        }



        private void dateofincident_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRTB();
        }


        private void DescriptionAccidentSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void policeYes_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void Police_ReportYes_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            PoliceReportNumberPanel.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            PoliceReportNumberPanel.Visibility = Visibility.Visible;
        }


        private void PassengerYesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void YesAttorneyButton_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            LawyerFollowUpQuestionsBorder.BeginAnimation(OpacityProperty, fadeInAnimation);
            LawyerFollowUpQuestionsBorder.Visibility = Visibility.Visible;
        }

        private void NoAttorneyButton_Checked(object sender, RoutedEventArgs e)
        {
            LawyerFollowUpQuestionsBorder.Visibility = Visibility.Collapsed;

            UpdateRTB();
        }

        private void PassengerNoButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

        }




        private async void policeYes_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

            PoliceFollowUpQuestions.Opacity = 0;
            PoliceFollowUpQuestions.Visibility = Visibility.Visible;
            FaultQuestion.Opacity = 0;
            FaultQuestion.Visibility = Visibility.Visible;

            DoubleAnimation fadeInPolice = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            DoubleAnimation fadeInFault = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            await Task.Delay(100);

            PoliceFollowUpQuestions.BeginAnimation(UIElement.OpacityProperty, fadeInPolice);
            FaultQuestion.BeginAnimation(UIElement.OpacityProperty, fadeInFault);
        }


        private void policeNo_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
            PoliceFollowUpQuestions.Visibility = Visibility.Collapsed;
        }

        private void Police_ReportNo_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
            PoliceReportNumberPanel.Visibility = Visibility.Collapsed;
        }

        private void PoliceReportNumberSubmit_Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void SettlementSubmitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LawyerSignDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRTB();
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            ApproxLawyerSignDate.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void DescriptionInjurySubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void DescriptionTreatmentSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void CityTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }



        private void Police_ReportIDK_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
            PoliceReportNumberPanel.Visibility = Visibility.Collapsed;
        }

        private void SettlementCompanySubmit_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void HavePolice_ReportYes_Checked(object sender, RoutedEventArgs e)
        {

            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            PRNumberInsertion.BeginAnimation(Border.OpacityProperty, fadeInAnimation);
            PRNumberInsertion.Visibility = Visibility.Visible;
            PoliceReportCopyPanel.Visibility = Visibility.Visible;


        }

        private void HavePolice_ReportNo_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
            PRNumberInsertion.Visibility = Visibility.Collapsed;
            PoliceReportCopyPanel.Visibility = Visibility.Collapsed;

        }

        private void Police_ReportNumber_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void DriverButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PassengerButton_Checked(object sender, RoutedEventArgs e)
        {

        }




        private void URScriptExit_Click(object sender, RoutedEventArgs e)
        {
            // Hide the pop-up container
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => URScriptPopupContainer.Visibility = Visibility.Collapsed;
            URScriptPopupContainer.BeginAnimation(OpacityProperty, fadeOutAnimation);

        }



        private void passengerSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void policeFaultIDK_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void URScriptPopupButton_Click(object sender, RoutedEventArgs e)
        {
            URScriptPopupContainer.Visibility = Visibility.Visible;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            URScriptPopupContainer.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void TDScriptPopupButton_Click(object sender, RoutedEventArgs e)
        {
            TDScriptPopupContainer.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            TDScriptPopupContainer.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void TDScriptExit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => TDScriptPopupContainer.Visibility = Visibility.Collapsed;
            TDScriptPopupContainer.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void ROScriptPopupButton_Click(object sender, RoutedEventArgs e)
        {
            ROScriptPopupContainer.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            ROScriptPopupContainer.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void ROScriptExit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => ROScriptPopupContainer.Visibility = Visibility.Collapsed;
            ROScriptPopupContainer.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void SUScriptPopupButton_Click(object sender, RoutedEventArgs e)
        {
            SUScriptPopupContainer.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            SUScriptPopupContainer.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void SUScriptExit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => SUScriptPopupContainer.Visibility = Visibility.Collapsed;
            SUScriptPopupContainer.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void DescriptionAccidentSubmitButton_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void DeterminingThePC_Click(object sender, RoutedEventArgs e)
        {
            DeterminingPCContainer.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            DeterminingPCContainer.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void PCExit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => DeterminingPCContainer.Visibility = Visibility.Collapsed;
            DeterminingPCContainer.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }



        private void PCisCalling_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

            TextRange textRange = new TextRange(tbDisplay.Document.ContentStart, tbDisplay.Document.ContentEnd);
            string text = textRange.Text;

            // Replace all occurrences of "IP" with "PC" in the text
            text = text.Replace("IP", "PC");

            tbDisplay.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph(new Run(text));
            tbDisplay.Document.Blocks.Add(paragraph);

            PCforIPFU.Visibility = Visibility.Collapsed;
            CforIPFU.Visibility = Visibility.Collapsed;
            CforPCFU.Visibility = Visibility.Collapsed;

            checkDoi.Visibility = Visibility.Visible;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            checkDoi.BeginAnimation(OpacityProperty, fadeInAnimation);

        }


        private void PCforIP_Checked_1(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

            PCforIPFU.Visibility = Visibility.Visible;
            CforIPFU.Visibility = Visibility.Collapsed;
            CforPCFU.Visibility = Visibility.Collapsed;
            checkDoi.Visibility = Visibility.Collapsed;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            PCforIPFU.BeginAnimation(OpacityProperty, fadeInAnimation);

        }

        private void TagsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = TagsListBox.SelectedItems;

            selectedBodyPains.Clear();

            foreach (var selectedItem in selectedItems)
            {
                string tag = ((ListBoxItem)selectedItem).Content.ToString();
                selectedBodyPains.Add(tag);
            }

            UpdateBrokenBoneDetails();

        }



        private List<String> selectedBones = new List<string>();
        private List<String> selectedBodyPains = new List<string>();
        private List<String> selectedOtherInjuries = new List<string>();

        private void SkullButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("skull"))
            {
                selectedBones.Remove("skull");
            }
            else
            {
                selectedBones.Add("skull");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("skull"))
            {
                SkullButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                SkullButton.Foreground = Brushes.White;
                CATnonCAT.Text = "A broken skull is a major broken bone. (CAT)";

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);


            }
            else
            {
                SkullButton.Background = Brushes.White;
                SkullButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void OrbitalButton_Click(object sender, RoutedEventArgs e)
        {

            if (selectedBones.Contains("orbital"))
            {
                selectedBones.Remove("orbital");
            }
            else
            {
                selectedBones.Add("orbital");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("orbital"))
            {
                OrbitalButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                OrbitalButton.Foreground = Brushes.White;
                CATnonCAT.Text = "A broken orbital bone is a major broken bone. (CAT)";

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                OrbitalButton.Background = Brushes.White;
                OrbitalButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void EyeSocketButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("eye socket"))
            {
                EyeSocketButton.Background = Brushes.White;
                EyeSocketButton.Foreground = Brushes.Black;
                selectedBones.Remove("eye socket");


            }
            else
            {
                EyeSocketButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                EyeSocketButton.Foreground = Brushes.White;
                selectedBones.Add("eye socket");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("eye socket"))
            {
                CATnonCAT.Text = "A broken eye socket is a major broken bone. (CAT)";

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
            }
        }

        private void ClavicleButton_Click(object sender, RoutedEventArgs e)
        {
            //index = 4

            if (selectedBones.Contains("clavicle"))
            {
                selectedBones.Remove("clavicle");
            }
            else
            {
                selectedBones.Add("clavicle");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("clavicle"))
            {
                CATnonCAT.Text = "A broken clavicle is a minor broken bone.";
                ClavicleButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                ClavicleButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                ClavicleButton.Background = Brushes.White;
                ClavicleButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }


        private void MandibleButton_Click(object sender, RoutedEventArgs e)
        {

            //index = 5

            if (selectedBones.Contains("mandible"))
            {
                selectedBones.Remove("mandible");
            }
            else
            {
                selectedBones.Add("mandible");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("mandible"))
            {

                CATnonCAT.Text = "A broken mandible is a minor broken bone.";
                MandibleButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                MandibleButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                MandibleButton.Background = Brushes.White;
                MandibleButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void JawButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("jaw"))
            {
                selectedBones.Remove("jaw");
            }
            else
            {
                selectedBones.Add("jaw");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("jaw"))
            {
                CATnonCAT.Text = "A broken jaw is a minor broken bone.";
                JawButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                JawButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                JawButton.Background = Brushes.White;
                JawButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void CervicelButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("cervical vertebrae"))
            {
                selectedBones.Remove("cervical vertebrae");
            }
            else
            {
                selectedBones.Add("cervical vertebrae");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("cervical vertebrae"))
            {
                CATnonCAT.Text = "A broken cervicel vertebrae is a major broken bone. (CAT)";
                CervicelButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                CervicelButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CervicelButton.Background = Brushes.White;
                CervicelButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void ThorasicButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("thorasic vertebrae"))
            {
                selectedBones.Remove("thorasic vertebrae");
            }
            else
            {
                selectedBones.Add("thorasic vertebrae");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("thorasic vertebrae"))
            {
                CATnonCAT.Text = "A broken thoracic vertebrae is a major broken bone. (CAT)";
                ThorasicButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                ThorasicButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                ThorasicButton.Background = Brushes.White;
                ThorasicButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void LumbarVertebraeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("lumbar vertebrae"))
            {
                selectedBones.Remove("lumbar vertebrae");
            }
            else
            {
                selectedBones.Add("lumbar vertebrae");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("lumbar vertebrae"))
            {
                LumbarVertebraeButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                LumbarVertebraeButton.Foreground = Brushes.White;
                CATnonCAT.Text = "A broken lumbar vertebrae is a major broken bone. (CAT)";

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                LumbarVertebraeButton.Background = Brushes.White;
                LumbarVertebraeButton.Foreground = Brushes.Black;
                CATnonCAT.Text = "";
            }
        }

        private void ElbowButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("elbow"))
            {
                selectedBones.Remove("elbow");
            }
            else
            {
                selectedBones.Add("elbow");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("elbow"))
            {
                CATnonCAT.Text = "If there was surgery done for the elbow, it's Major. If not it's minor.";
                ElbowButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                ElbowButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                ElbowButton.Background = Brushes.White;
                ElbowButton.Foreground = Brushes.Black;
            }
        }

        private void PelvisButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("pelvis"))
            {
                selectedBones.Remove("pelvis");
            }
            else
            {
                selectedBones.Add("pelvis");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("pelvis"))
            {
                CATnonCAT.Text = "A broken pelvis is a major broken bone. (CAT)";
                PelvisButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                PelvisButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                PelvisButton.Background = Brushes.White;
                PelvisButton.Foreground = Brushes.Black;
            }
        }

        private void SacrumButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("sacrum"))
            {
                selectedBones.Remove("sacrum");
            }
            else
            {
                selectedBones.Add("sacrum");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("sacrum"))
            {
                CATnonCAT.Text = "A broken sacrum is a major broken bone. (CAT)";
                SacrumButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                SacrumButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                SacrumButton.Background = Brushes.White;
                SacrumButton.Foreground = Brushes.Black;
            }
        }

        private void ScapulaButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("scapula"))
            {
                selectedBones.Remove("scapula");
            }
            else
            {
                selectedBones.Add("scapula");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("scapula"))
            {
                CATnonCAT.Text = "A broken scapula is a minor broken bone.";
                ScapulaButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                ScapulaButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                ScapulaButton.Background = Brushes.White;
                ScapulaButton.Foreground = Brushes.Black;
            }
        }

        private void ShoulderButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("shoulder"))
            {
                selectedBones.Remove("shoulder");
            }
            else
            {
                selectedBones.Add("shoulder");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("shoulder"))
            {
                CATnonCAT.Text = "If there was surgery done for the shoulder, it's Major. If not it's minor.";
                ShoulderButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                ShoulderButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                ShoulderButton.Background = Brushes.White;
                ShoulderButton.Foreground = Brushes.Black;
            }
        }

        private void SternumButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("sternum"))
            {
                selectedBones.Remove("sternum");
            }
            else
            {
                selectedBones.Add("sternum");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("sternum"))
            {
                CATnonCAT.Text = "A broken sternum is a minor broken bone.";
                SternumButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                SternumButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                SternumButton.Background = Brushes.White;
                SternumButton.Foreground = Brushes.Black;
            }
        }

        private void RibsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("rib"))
            {
                selectedBones.Remove("rib");
            }
            else
            {
                selectedBones.Add("rib");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("rib"))
            {
                CATnonCAT.Text = "A broken rib is a minor broken bone.";
                RibsButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                RibsButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                RibsButton.Background = Brushes.White;
                RibsButton.Foreground = Brushes.Black;
            }
        }

        private void HumerusButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("humerus"))
            {
                selectedBones.Remove("humerus");
            }
            else
            {
                selectedBones.Add("humerus");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("humerus"))
            {
                CATnonCAT.Text = "If there was surgery done for the humerus, it's Major. If not it's minor.";
                HumerusButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                HumerusButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                HumerusButton.Background = Brushes.White;
                HumerusButton.Foreground = Brushes.Black;
            }
        }

        private void RadiusButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("radius"))
            {
                selectedBones.Remove("radius");
            }
            else
            {
                selectedBones.Add("radius");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("radius"))
            {
                CATnonCAT.Text = "If there was surgery done for the radius, it's Major. If not it's minor.";
                RadiusButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                RadiusButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                RadiusButton.Background = Brushes.White;
                RadiusButton.Foreground = Brushes.Black;
            }
        }

        private void FemurButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("femur"))
            {
                selectedBones.Remove("femur");
            }
            else
            {
                selectedBones.Add("femur");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("femur"))
            {
                CATnonCAT.Text = "A broken femur is a major broken bone. (CAT)";
                FemurButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                FemurButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                FemurButton.Background = Brushes.White;
                FemurButton.Foreground = Brushes.Black;

            }
        }

        private void FibulaButton_Click(object sender, RoutedEventArgs e)
        {

            if (selectedBones.Contains("fibula"))
            {
                selectedBones.Remove("fibula");
            }
            else
            {
                selectedBones.Add("fibula");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("fibula"))
            {
                CATnonCAT.Text = "A broken fibula is a major broken bone. (CAT)";
                FibulaButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                FibulaButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                FibulaButton.Background = Brushes.White;
                FibulaButton.Foreground = Brushes.Black;

            }
        }

        private void MetacarpalsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("metacarpal"))
            {
                selectedBones.Remove("metacarpal");
            }
            else
            {
                selectedBones.Add("metacarpal");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("metacarpal"))
            {
                CATnonCAT.Text = "A broken metacarpals is a minor broken bone.";
                MetacarpalsButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                MetacarpalsButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                MetacarpalsButton.Background = Brushes.White;
                MetacarpalsButton.Foreground = Brushes.Black;

            }
        }

        private void MetatarsalsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("metatarsal"))
            {
                selectedBones.Remove("metatarsal");
            }
            else
            {
                selectedBones.Add("metatarsal");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("metatarsal"))
            {
                CATnonCAT.Text = "A broken metatarsal bone is a minor broken bone.";
                MetatarsalsButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                MetatarsalsButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                MetatarsalsButton.Background = Brushes.White;
                MetatarsalsButton.Foreground = Brushes.Black;

            }
        }

        private void PatellaButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("patella"))
            {
                selectedBones.Remove("patella");
            }
            else
            {
                selectedBones.Add("patella");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("patella"))
            {
                CATnonCAT.Text = "A broken patella is a major broken bone. (CAT)";
                PatellaButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                PatellaButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                PatellaButton.Background = Brushes.White;
                PatellaButton.Foreground = Brushes.Black;

            }
        }

        private void PhalangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("phalange"))
            {
                selectedBones.Remove("phalange");
            }
            else
            {
                selectedBones.Add("phalange");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("phalange"))
            {
                CATnonCAT.Text = "A broken phalange is a minor broken bone.";
                PhalangesButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                PhalangesButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                PhalangesButton.Background = Brushes.White;
                PhalangesButton.Foreground = Brushes.Black;

            }
        }

        private void TarsalsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("tarsal"))
            {
                selectedBones.Remove("tarsal");
            }
            else
            {
                selectedBones.Add("tarsal");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("tarsal"))
            {
                CATnonCAT.Text = "A broken tarsal bone is a minor broken bone.";
                TarsalsButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                TarsalsButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                TarsalsButton.Background = Brushes.White;
                TarsalsButton.Foreground = Brushes.Black;

            }
        }

        private void TibiaButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("tibia"))
            {
                selectedBones.Remove("tibia");
            }
            else
            {
                selectedBones.Add("tibia");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("tibia"))
            {
                CATnonCAT.Text = "A broken tibia is a major broken bone. (CAT)";
                TibiaButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                TibiaButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                TibiaButton.Background = Brushes.White;
                TibiaButton.Foreground = Brushes.Black;

            }
        }

        private void UlnaButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("ulna"))
            {
                selectedBones.Remove("ulna");
            }
            else
            {
                selectedBones.Add("ulna");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("ulna"))
            {
                CATnonCAT.Text = "If there was surgery done for the ulna, it's Major. If not it's minor.";
                UlnaButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                UlnaButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                UlnaButton.Background = Brushes.White;
                UlnaButton.Foreground = Brushes.Black;

            }
        }

        private void CoccyxButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("coccyx"))
            {
                selectedBones.Remove("coccyx");
            }
            else
            {
                selectedBones.Add("coccyx");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("coccyx"))
            {
                CATnonCAT.Text = "A broken coccyx is a minor broken bone.";
                CoccyxButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                CoccyxButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                CoccyxButton.Background = Brushes.White;
                CoccyxButton.Foreground = Brushes.Black;

            }
        }

        private void WristButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("wrist"))
            {
                selectedBones.Remove("wrist");
            }
            else
            {
                selectedBones.Add("wrist");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("wrist"))
            {
                CATnonCAT.Text = "A broken wrist is a minor broken bone.";
                WristButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                WristButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                WristButton.Background = Brushes.White;
                WristButton.Foreground = Brushes.Black;

            }
        }

        private void CarpalsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("carpal"))
            {
                selectedBones.Remove("carpal");
            }
            else
            {
                selectedBones.Add("carpal");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("carpal"))
            {
                CATnonCAT.Text = "A broken carpal is a minor broken bone.";
                CarpalsButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                CarpalsButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                CarpalsButton.Background = Brushes.White;
                CarpalsButton.Foreground = Brushes.Black;

            }
        }

        private void HipButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("hip"))
            {
                selectedBones.Remove("hip");
            }
            else
            {
                selectedBones.Add("hip");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("hip"))
            {
                CATnonCAT.Text = "A broken hip is a major broken bone. (CAT)";
                HipButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                HipButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                HipButton.Background = Brushes.White;
                HipButton.Foreground = Brushes.Black;

            }
        }

        private void AnkleButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("ankle"))
            {
                selectedBones.Remove("ankle");
            }
            else
            {
                selectedBones.Add("ankle");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("ankle"))
            {
                CATnonCAT.Text = "A broken ankle is a minor broken bone.";
                AnkleButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                AnkleButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                AnkleButton.Background = Brushes.White;
                AnkleButton.Foreground = Brushes.Black;

            }
        }

        private void PhalangesFeetButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBones.Contains("phalange (feet)"))
            {
                selectedBones.Remove("phalange (feet)");
            }
            else
            {
                selectedBones.Add("phalange (feet)");
            }

            UpdateBrokenBoneDetails();

            if (selectedBones.Contains("phalange (feet)"))
            {
                CATnonCAT.Text = "A broken phalange in the feet is a minor broken bone.";
                PhalangesFeetButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                PhalangesFeetButton.Foreground = Brushes.White;

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                };
                CATnonCAT.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);

            }
            else
            {
                CATnonCAT.Text = "";
                PhalangesFeetButton.Background = Brushes.White;
                PhalangesFeetButton.Foreground = Brushes.Black;

            }
        }



        string sInjuryBullets, sInjuryDetails = "";

        string sBrokenBones;
        string sBodyPains;
        string sOtherInjuries;

        private void UpdateBrokenBoneDetails()
        {
            sBrokenBones = String.Join(", ", selectedBones.Select(x => x.ToString()).ToArray());
            sBodyPains = String.Join(", ", selectedBodyPains.Select(x => x.ToString()).ToArray());
            sOtherInjuries = String.Join(", ", selectedOtherInjuries.Select(x => x.ToString()).ToArray());

            int intIndex = sBrokenBones.LastIndexOf(",");
            if (intIndex > -1)
            {
                sBrokenBones = sBrokenBones.Remove(intIndex, 1).Insert(intIndex, " and");
            }



            intIndex = sBodyPains.LastIndexOf(",");
            if (intIndex > -1)
            {
                sBodyPains = sBodyPains.Remove(intIndex, 1).Insert(intIndex, " and");
            }


            intIndex = sOtherInjuries.LastIndexOf(",");
            if (intIndex > -1)
            {
                sOtherInjuries = sOtherInjuries.Remove(intIndex, 1).Insert(intIndex, " and");
            }




            sInjuryDetails = (selectedBones.Count > 0) ? "IP has a broken " + sBrokenBones.ToLower() + ". " : "";
            sInjuryDetails = (selectedBodyPains.Count > 0) ? sInjuryDetails + "IP feels pain in their " + sBodyPains.ToLower() + ". " : sInjuryDetails;




            string otherInjuriesSentence = "";

            if (selectedOtherInjuries.Count > 0)
            {
                otherInjuriesSentence = "IP sustained ";

                // Create a list of injury names without abbreviations
                List<string> injuryNames = new List<string>();
                foreach (string injury in selectedOtherInjuries)
                {
                    switch (injury)
                    {
                        case "Coma":
                            injuryNames.Add("a coma");
                            break;
                        case "Loss of Limb":
                            injuryNames.Add("a loss of limb");
                            break;
                        case "Concussion":
                            injuryNames.Add("a concussion");
                            break;
                        case "Brief LOC":
                            injuryNames.Add("a brief loss of consciousness");
                            break;
                        case "Loss of Hearing":
                            injuryNames.Add("a loss of hearing");
                            break;
                        case "Heart attack":
                            injuryNames.Add("a heart attack");
                            break;
                        case "Herniated Disc":
                            injuryNames.Add("a herniated disc");
                            break;
                        case "Bulging Disc":
                            injuryNames.Add("a bulging disc");
                            break;
                        case "Ruptured disc":
                            injuryNames.Add("a ruptured disc");
                            break;
                        case "TBI":
                            injuryNames.Add("a traumatic brain injury");
                            break;
                        case "Stroke":
                            injuryNames.Add("a stroke");
                            break;
                        case "Amputation":
                            injuryNames.Add("an amputation");
                            break;
                        case "ELOC":
                            injuryNames.Add("an extended loss of consciousness");
                            break;
                        case "PTSD":
                            injuryNames.Add("post-traumatic stress disorder");
                            break;
                        // Add more cases for other abbreviations
                        default:

                            injuryNames.Add(injury.ToLower());
                            break;
                    }
                }

                // Combine injury names into a sentence
                if (injuryNames.Count == 1)
                {
                    otherInjuriesSentence += injuryNames[0] + ". ";
                }
                else if (injuryNames.Count == 2)
                {
                    otherInjuriesSentence += injuryNames[0] + " and " + injuryNames[1] + ". ";
                }
                else
                {
                    for (int i = 0; i < injuryNames.Count - 1; i++)
                    {
                        otherInjuriesSentence += injuryNames[i] + ", ";
                    }
                    otherInjuriesSentence += "and " + injuryNames[injuryNames.Count - 1] + ". ";
                }

                sInjuryDetails += otherInjuriesSentence;
            }



            if ((selectedOtherInjuries.Count > 0) || (selectedBodyPains.Count > 0) || (selectedBones.Count > 0))
            {
                sInjuryBullets = "\n\nInjuries \n";
            }
            else
            {
                sInjuryBullets = "";
            }

            if (DeathButton.IsChecked == true)
            {
                sInjuryDetails += "\nIP passed away.";

            }

            if (NoneButton.IsChecked == true)
            {
                sInjuryDetails = "At the moment, IP has sustained no injuries related to the accident.";
            }



            foreach (var bone in selectedBones)
            {
                sInjuryBullets += ("- Broken " + bone + "\n");
            }

            foreach (var pain in selectedBodyPains)
            {
                sInjuryBullets += ("- " + pain + " pain" + "\n");
            }

            foreach (var injuries in selectedOtherInjuries)
            {
                sInjuryBullets += ("- " + injuries + "\n");
            }

            if ((sInjuryBullets == "") && (DeathButton.IsChecked == true))
            {
                sInjuryBullets = "\n\nInjuries:\n- Death";
            }
            else if ((sInjuryBullets != "") && (DeathButton.IsChecked == true))
            {
                sInjuryBullets += "- Death\n";
            }




            if ((selectedOtherInjuries.Count == 0) && (selectedBodyPains.Count == 0) && (selectedBones.Count == 0))
            {
                InjuryDetailsTextBox.Clear();
            }

            if (NoneButton.IsChecked == true)
            {
                sInjuryBullets = ("\n\nInjuries \n- None\n");
            }

            InjuryDetailsTextBox.Text = sInjuryDetails;
            InjuryDetailsTextBox.Text += sInjuryBullets;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            InjuryDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);


        }

        private List<string> selectedReasonsFornoTreatments = new List<string>();

        string explanation = "";
        private void WhynoTreatmentExplainUpdate()
        {

            if (selectedReasonsFornoTreatments.Contains("Financial Constraints"))
            {
                explanation = "IP has not gone for treatment due to financial constraints. ";
            }

            if (selectedReasonsFornoTreatments.Contains("Insurance Complications"))
            {
                explanation = "IP has not gone for treatment due to insurance complications. ";
            }

            if (selectedReasonsFornoTreatments.Contains("No available appointments"))
            {
                explanation = "IP has not gone for treatment due to appointments not being available. ";
            }

            if (selectedReasonsFornoTreatments.Contains("Not willing to get treated"))
            {
                explanation = "IP is not willing to get treated at the moment. ";
            }

            if (selectedReasonsFornoTreatments.Contains("Lack of transportation"))
            {
                explanation = "IP has not gone for treatment due to a lack of transportation. ";
            }

            if (selectedReasonsFornoTreatments.Contains("Accident has just happened"))
            {
                explanation = "IP has not gone for treatment due to the accident being so recent. ";
            }


            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };

            ExplanationTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);
            ExplanationTextBox.Text = explanation;

        }

        private void PossibleReasons1_Click(object sender, RoutedEventArgs e)
        {
            selectedReasonsFornoTreatments.Add("Financial Constraints");
            selectedReasonsFornoTreatments.Remove("Insurance Complications");
            selectedReasonsFornoTreatments.Remove("No available appointments");
            selectedReasonsFornoTreatments.Remove("Not willing to get treated");
            selectedReasonsFornoTreatments.Remove("Lack of transportation");
            selectedReasonsFornoTreatments.Remove("Accident has just happened");


            WhynoTreatmentExplainUpdate();
        }

        private void PossibleReasons2_Click(object sender, RoutedEventArgs e)
        {
            selectedReasonsFornoTreatments.Add("Insurance Complications");
            selectedReasonsFornoTreatments.Remove("Financial Constraints");
            selectedReasonsFornoTreatments.Remove("No available appointments");
            selectedReasonsFornoTreatments.Remove("Not willing to get treated");
            selectedReasonsFornoTreatments.Remove("Lack of transportation");
            selectedReasonsFornoTreatments.Remove("Accident has just happened");

            WhynoTreatmentExplainUpdate();
        }

        private void PossibleReasons3_Click(object sender, RoutedEventArgs e)
        {
            selectedReasonsFornoTreatments.Add("No available appointments");
            selectedReasonsFornoTreatments.Remove("Insurance Complications");
            selectedReasonsFornoTreatments.Remove("Financial Constraints");
            selectedReasonsFornoTreatments.Remove("Not willing to get treated");
            selectedReasonsFornoTreatments.Remove("Lack of transportation");
            selectedReasonsFornoTreatments.Remove("Accident has just happened");


            WhynoTreatmentExplainUpdate();
        }

        private void PossibleReasons4_Click(object sender, RoutedEventArgs e)
        {
            selectedReasonsFornoTreatments.Add("Not willing to get treated");
            selectedReasonsFornoTreatments.Remove("Insurance Complications");
            selectedReasonsFornoTreatments.Remove("Financial Constraints");
            selectedReasonsFornoTreatments.Remove("No available appointments");
            selectedReasonsFornoTreatments.Remove("Lack of transportation");
            selectedReasonsFornoTreatments.Remove("Accident has just happened");

            WhynoTreatmentExplainUpdate();
        }

        private void PossibleReasons5_Click(object sender, RoutedEventArgs e)
        {
            selectedReasonsFornoTreatments.Add("Lack of transportation");
            selectedReasonsFornoTreatments.Remove("Insurance Complications");
            selectedReasonsFornoTreatments.Remove("Financial Constraints");
            selectedReasonsFornoTreatments.Remove("No available appointments");
            selectedReasonsFornoTreatments.Remove("Not willing to get treated");
            selectedReasonsFornoTreatments.Remove("Accident has just happened");



            WhynoTreatmentExplainUpdate();
        }

        private void PossibleReasons6_Click(object sender, RoutedEventArgs e)
        {
            selectedReasonsFornoTreatments.Add("Accident has just happened");
            selectedReasonsFornoTreatments.Remove("Insurance Complications");
            selectedReasonsFornoTreatments.Remove("Financial Constraints");
            selectedReasonsFornoTreatments.Remove("No available appointments");
            selectedReasonsFornoTreatments.Remove("Not willing to get treated");
            selectedReasonsFornoTreatments.Remove("Lack of transportation");


            WhynoTreatmentExplainUpdate();
        }



        List<string> slDoctorNames = new List<string>();


        string FinalTreatmentDetails = "";

        string sTreatmentBullets = "";

        private void UpdateTreatments()
        {
            slDoctorNames.Clear(); // Clear the list before adding new items

            foreach (ListBoxItem item in DoctorsSpecialistsTagListBox.SelectedItems)
            {
                slDoctorNames.Add(item.Content.ToString().ToLower());
            }

            if (selectedCommonTreatments.Contains("Chiropractor"))
            {
                slDoctorNames.Add("chiropractor");
            }

            if (selectedCommonTreatments.Contains("PrimaryCare"))
            {
                slDoctorNames.Add("primary care physician");
            }

            string sDoctors = String.Join(", ", slDoctorNames.Distinct().Select(x => x.ToString()).ToArray());
            string[] doctors = sDoctors.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            string sArticle = "a";

            if (doctors.Length > 0 && (doctors[0].StartsWith("a") || doctors[0].StartsWith("e") ||
                doctors[0].StartsWith("i") || doctors[0].StartsWith("o") || doctors[0].StartsWith("u")))
            {
                sArticle = "an";
            }

            string sTreatmentDetails = "";

            if (IPNotTreatedWithin14daysCheckBox.IsChecked == true)
            {
                sTreatmentDetails += "*IP did not get treatment within 14 days of the accident.\n\n";
            }

            if (doctors.Length > 0)
            {
                sTreatmentDetails += "IP visited " + sArticle + " " + doctors[0];

                if (doctors.Length > 1)
                {
                    for (int i = 1; i < doctors.Length - 1; i++)
                    {
                        sArticle = "a";
                        if (doctors[i].StartsWith("a") || doctors[i].StartsWith("e") || doctors[i].StartsWith("i") ||
                            doctors[i].StartsWith("o") || doctors[i].StartsWith("u"))
                        {
                            sArticle = "an";
                        }

                        sTreatmentDetails += ", " + sArticle + " " + doctors[i];
                    }

                    // Handle the last doctor separately to add the conjunction "and"
                    sArticle = "a";
                    if (doctors[doctors.Length - 1].StartsWith("a") || doctors[doctors.Length - 1].StartsWith("e") ||
                        doctors[doctors.Length - 1].StartsWith("i") || doctors[doctors.Length - 1].StartsWith("o") ||
                        doctors[doctors.Length - 1].StartsWith("u"))
                    {
                        sArticle = "an";
                    }
                    sTreatmentDetails += ", and " + sArticle + " " + doctors[doctors.Length - 1];
                }

                sTreatmentDetails += ". ";
            }


            string doctorsBulletPoints = "";
            foreach (string doctor in slDoctorNames)
            {
                doctorsBulletPoints += "- " + doctor + "\n";
            }


            if (selectedCommonTreatments.Contains("Ambulance") && selectedCommonTreatments.Contains("ER"))
            {
                sTreatmentDetails += "IP was transported to the ER in an ambulance. ";
            }
            else if (selectedCommonTreatments.Contains("ER"))
            {
                sTreatmentDetails += "IP went to the ER. ";
            }

            if (selectedCommonTreatments.Contains("Urgent Care"))
            {
                sTreatmentDetails += "IP went to an urgent care facility. ";
            }

            if (selectedCommonTreatments.Contains("CT Scan"))
            {
                sTreatmentDetails += "IP had a CT scan done";

                if (selectedCommonTreatments.Contains("MRI"))
                {
                    sTreatmentDetails += ", as well an MRI";

                    if (selectedCommonTreatments.Contains("Xray"))
                    {
                        sTreatmentDetails += ", and an X-ray. ";
                    }
                    else
                    {
                        sTreatmentDetails += ". ";
                    }
                }
                else if (selectedCommonTreatments.Contains("Xray"))
                {
                    sTreatmentDetails += " as well as an X-ray. ";
                }
                else
                {
                    sTreatmentDetails += ". ";
                }
            }
            else if (selectedCommonTreatments.Contains("MRI"))
            {
                sTreatmentDetails += "IP had an MRI scan done";

                if (selectedCommonTreatments.Contains("Xray"))
                {
                    sTreatmentDetails += ", as well an X-ray. ";
                }
                else
                {
                    sTreatmentDetails += ". ";
                }
            }
            else if (selectedCommonTreatments.Contains("Xray"))
            {
                sTreatmentDetails += "IP had an X-ray done. ";
            }

            if (selectedCommonTreatments.Contains("OTC Meds"))
            {
                sTreatmentDetails += "IP has taken OTC medication. ";
            }

            if (selectedCommonTreatments.Contains("RX Meds"))
            {
                sTreatmentDetails += "IP was given Rx medication";

                if (selectedCommonTreatments.Contains("Injections"))
                {
                    sTreatmentDetails += ", and received an injection as well. ";
                }
                else
                {
                    sTreatmentDetails += ". ";
                }
            }
            else if (selectedCommonTreatments.Contains("Injections"))
            {
                sTreatmentDetails += "IP was also given an injection. ";
            }

            if (selectedCommonTreatments.Contains("PT"))
            {
                sTreatmentDetails += "IP also attended physical therapy. ";
            }

            sTreatmentBullets = "\nTreatments: \n";

            sTreatmentBullets += doctorsBulletPoints;


            foreach (string treatment in selectedCommonTreatments)
            {
                if (treatment == "Chiropractor" || treatment == "PrimaryCare")
                {
                    continue;
                }
                sTreatmentBullets += "- " + treatment + "\n";
            }

            string TreatmentLineBreak = "\n";
            if (NoTreatmentButton.IsChecked == true)
            {
                sTreatmentDetails = "So far, IP has not received any medical treatment for the accident. " + explanation;
                sTreatmentBullets = ("\nTreatments: \n- None, " + explanation);
            }

            FinalTreatmentDetails = sTreatmentDetails;

            TreatmentDetailsTextBox.Text = sTreatmentDetails;
            TreatmentDetailsTextBox.Text += TreatmentLineBreak;
            TreatmentDetailsTextBox.Text += sTreatmentBullets;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            TreatmentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);
        }





        private void TagSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TagSearchBox.Text.ToLower();

            foreach (ListBoxItem item in TagsListBox.Items)
            {
                if (item.Content.ToString().ToLower().Contains(searchText))
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }



        private DispatcherTimer countdown;
        private int minutes = 4;
        private int seconds = 0;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            timerTextBlock.Text = $"{minutes:00}:{seconds:00}";

            // load the sound file
            mediaPlayer.Open(new Uri(@"C:\Users\14074\source\repos\WpfApp1\WpfApp1\tones.mp3"));
        }



        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            countdown = new DispatcherTimer();
            countdown.Interval = new TimeSpan(0, 0, 1);
            countdown.Tick += countdown_Tick;
            countdown.Start();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (countdown != null)
            {
                countdown.Stop();
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            if (countdown != null)
            {
                countdown.Stop();
            }

            minutes = 4;
            seconds = 0;
            timerTextBlock.Text = $"{minutes:00}:{seconds:00}";
        }

        private void countdown_Tick(object sender, EventArgs e)
        {
            if (seconds == 0)
            {
                minutes--;
                seconds = 59;
            }

            else
            {
                seconds--;
            }

            if (minutes == 0 && seconds <= 15)
            {
                mediaPlayer.Play();

                // play the sound continuously if there are 15 seconds or less left
                while (minutes == 0 && seconds <= 15 && mediaPlayer.Position >= mediaPlayer.NaturalDuration)
                {
                    mediaPlayer.Position = TimeSpan.Zero;
                    mediaPlayer.Play();
                    Thread.Sleep(1000); // wait for a short time before playing again
                }
            }

            if (minutes == 0 && seconds == 0)
            {
                countdown.Stop();
            }

            timerTextBlock.Text = $"{minutes:00}:{seconds:00}";
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (minutes == 0 && seconds <= 15)
            {
                mediaPlayer.Play();
            }
        }

        private void BackToTopButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollBar.ScrollToTop();
        }


        private void resetUIButton_Click(object sender, RoutedEventArgs e)
        {
            {
                tbDisplay.Document.Blocks.Clear();
                tbDisplay.Document.Blocks.Add(new Paragraph(new Run("DOI")));
                // Set the selected button to null
                selectedButton = null;

                // Set the Tag property of each button to "notSelected"
                btnRearEnd.Tag = "notSelected";
                btnHeadOn.Tag = "notSelected";
                btnSideswipe.Tag = "notSelected";
                btnTbone.Tag = "notSelected";

                // Clear the accident details text box
                AccidentDetailsTextBox.Text = "";

                // Hide the likely scenarios panels
                LikelyScenariosRear_End.Visibility = Visibility.Collapsed;
                LikelyScenariosHeadOn.Visibility = Visibility.Collapsed;
                LikelyScenariosSideSwipe.Visibility = Visibility.Collapsed;
                LikelyScenariosTbone.Visibility = Visibility.Collapsed;

                // Set the backgrounds of btnRearEnd and btnHeadOn to their initial colors
                btnRearEnd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                btnHeadOn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                btnSideswipe.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                btnTbone.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));

                // Clear the values of all the controls
                LawyerFollowUpQuestionsBorder.Visibility = Visibility.Collapsed;
                PoliceFollowUpQuestions.Visibility = Visibility.Collapsed;
                DoctorsSpecialistsTagListBox.SelectedItems.Clear();
                TagsListBox.SelectedItems.Clear();
                selectedBones.Clear();
                selectedBodyPains.Clear();
                selectedReasonsFornoTreatments.Clear();
                Police_ReportIDK.IsChecked = false;
                LikelyScenariosRear_End.Visibility = Visibility.Collapsed;
                LikelyScenariosHeadOn.Visibility = Visibility.Collapsed;
                LikelyScenariosTbone.Visibility = Visibility.Collapsed;
                LikelyScenariosSideSwipe.Visibility = Visibility.Collapsed;
                DriverFUQuestion1.Visibility = Visibility.Collapsed;
                DriverFUQuestion2.Visibility = Visibility.Collapsed;
                PassengerFUQuestion1.Visibility = Visibility.Collapsed;
                PassengerFUQuestion2.Visibility = Visibility.Collapsed;
                MassTransitFUQuestion1.Visibility = Visibility.Collapsed;
                InjuriesTagListBox.SelectedItems.Clear();
                noneCheckBox.IsChecked = false;
                StateBarComboBox.Text = string.Empty;
                NoneButton.IsChecked = false;
                NoTreatmentButton.IsChecked = false;
                selectedButton = null;
                AccidentDetailsTextBox.Text = "";
                FaultQuestion.Visibility = Visibility.Collapsed;
                PoliceFollowUpQuestions.Visibility = Visibility.Collapsed;
                PoliceReportNumberPanel.Visibility = Visibility.Collapsed;
                PoliceReportCopyPanel.Visibility = Visibility.Collapsed;
                HavePolice_CopyYes.IsChecked = false;
                HavePolice_CopyNo.IsChecked = false;
                PRNumberInsertion.Visibility = Visibility.Collapsed;
                TypeOfAccident.Text = string.Empty;
                CityofIncident.Text = string.Empty;
                StateOfIncident.Text = string.Empty;
                DateOfIncident.Text = string.Empty;
                IPWasDriver.IsChecked = false;
                IPWasMassTransitPassenger.IsChecked = false;
                IPWasPassenger.IsChecked = false;
                IPWasPedestrian.IsChecked = false;
                IPWasCyclist.IsChecked = false;
                DriverFUQuestion1Yes.IsChecked = false;
                DriverFUQuestion1No.IsChecked = false;
                IPDriverPassengerRelations.Text = string.Empty;
                IPPassengerDriverRelations.Text = string.Empty;
                HavePolice_ReportYes.IsChecked = false;
                HavePolice_ReportNo.IsChecked = false;
                YesAttorneyButton.IsChecked = false;
                NoAttorneyButton.IsChecked = false;
                LawyerNameTextBox.Text = string.Empty;
                LawyerSignDatePicker.SelectedDate = null;
                CaseProgressTextBox.Text = string.Empty;
                AccidentDetailsTextBox.Text = string.Empty;
                InjuryDetailsTextBox.Text = string.Empty;
                sInjuryDetails = "";
                sInjuryBullets = "";
                FinalTreatmentDetails = "";
                sTreatmentBullets = "";
                TreatmentDetailsTextBox.Text = string.Empty;
                policeYes.IsChecked = false;
                policeNo.IsChecked = false;
                policeFaultOP.IsChecked = false;
                policeFaultPC.IsChecked = false;
                policeFaultIDK.IsChecked = false;
                HavePolice_ReportYes.IsChecked = false;
                HavePolice_ReportNo.IsChecked = false;
                PCisCalling.IsChecked = false;
                PCforIP.IsChecked = false;
                CforPC.IsChecked = false;
                CforIP.IsChecked = false;
                Police_ReportNo.IsChecked = false;
                Police_ReportYes.IsChecked = false;
                PoliceReportNumberTextBox.Text = string.Empty;
                PublicTransportSystem.Text = string.Empty;
                DriverFUQuestion1Yes.IsChecked = false;
                DriverFUQuestion1No.IsChecked = false;
                IPDriverPassengerRelations.Text = string.Empty;
                selectedReasonsFornoTreatments.Clear();
                AirbagsCheckBox.IsChecked = false;
                CarServiceCheckBox.IsChecked = false;
                IPWorkingCheckBox.IsChecked = false;
                TeslaCheckBox.IsChecked = false;
                CarExplodedCheckBox.IsChecked = false;
                TireExplodedCheckBox.IsChecked = false;
                DUIOtherDriverCheckBox.IsChecked = false;
                CommercialVehicleCheckBox.IsChecked = false;
                SettlementOfferCheckBox.IsChecked = false;
                OtherCheckBox.IsChecked = false;
                AirbagsMessage.Visibility = Visibility.Collapsed;
                UberFUQuestions.Visibility = Visibility.Collapsed;
                IPWorkingPanel.Visibility = Visibility.Collapsed;
                CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;
                CommercialVehiclePanel.Visibility = Visibility.Collapsed;
                SettlementOfferPanel.Visibility = Visibility.Collapsed;
                OtherInformationStackpanel.Visibility = Visibility.Collapsed;
                DriverinCarService.IsChecked = false;
                PassengerinCarService.IsChecked = false;
                IPsCarServiceCompany.Text = string.Empty;
                IPJobComboBox.SelectedItem = null;
                IPCompanyTextBox.Text = string.Empty;
                CommercialVehicleComboBox.SelectedItem = null;
                SettlementOfferFromTextBox.Text = string.Empty;
                SettlementOfferAmountTextBox.Text = string.Empty;
                OtherInformation.Text = string.Empty;
                policeFaultPCadmits.IsChecked = false;
                IPPassengerPassengersOtherThanDriver.IsChecked = false;
                IPPassengerPassengersOtherThanDriverNO.IsChecked = false;
                IPPassengerOtherPassengerRelations.Text = string.Empty;

                // Find ambulanceBorder
                Border ambulanceBorder = FindVisualChild<Border>(AmbulanceButton);

                    // Find ERborder
                    Border erBorder = FindVisualChild<Border>(ERbutton);

                    // Find urgentCareBorder
                    Border urgentCareBorder = FindVisualChild<Border>(UrgentCare);

                    // Find xrayBorder
                    Border xrayBorder = FindVisualChild<Border>(XrayButton);

                    // Find ctScanBorder
                    Border ctScanBorder = FindVisualChild<Border>(CTScanButton);

                    // Find RXborder
                    Border rxBorder = FindVisualChild<Border>(RXMeds);

                    // Find otcBorder
                    Border otcBorder = FindVisualChild<Border>(OTCs);

                    // Find chiropractorBorder
                    Border chiropractorBorder = FindVisualChild<Border>(ChiropractorButton);

                    // Find primaryCareBorder
                    Border primaryCareBorder = FindVisualChild<Border>(PrimaryCare);

                    // Find mriBorder
                    Border mriBorder = FindVisualChild<Border>(MRIButton);

                    // Find ptBorder
                    Border ptBorder = FindVisualChild<Border>(PhysicalTherapy);

                    // Find injBorder
                    Border injBorder = FindVisualChild<Border>(Injections);

                    // Clear slDoctorNames list
                    slDoctorNames.Clear();

                    // Clear selected items in DoctorsSpecialistsTagListBox
                    DoctorsSpecialistsTagListBox.SelectedItems.Clear();

                    // Uncheck IPNotTreatedWithin14daysCheckBox
                    IPNotTreatedWithin14daysCheckBox.IsChecked = false;

                // Clear sTreatmentDetails string
                    FinalTreatmentDetails = string.Empty;

                    // Clear sTreatmentBullets string
                    sTreatmentBullets = string.Empty;

                    if (selectedCommonTreatments.Contains("Ambulance"))
                    {
                        selectedCommonTreatments.Remove("Ambulance");
                        ambulanceBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("ER"))
                    {
                        selectedCommonTreatments.Remove("ER");
                        erBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("Urgent Care"))
                    {
                        selectedCommonTreatments.Remove("Urgent Care");
                        urgentCareBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("Xray"))
                    {
                        selectedCommonTreatments.Remove("Xray");
                        xrayBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("CT Scan"))
                    {
                        selectedCommonTreatments.Remove("CT Scan");
                        ctScanBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("RX Meds"))
                    {
                        selectedCommonTreatments.Remove("RX Meds");
                        rxBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("OTC Meds"))
                    {
                        selectedCommonTreatments.Remove("OTC Meds");
                        otcBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("Chiropractor"))
                    {
                        selectedCommonTreatments.Remove("Chiropractor");
                        chiropractorBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("PrimaryCare"))
                    {
                        selectedCommonTreatments.Remove("PrimaryCare");
                        primaryCareBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("MRI"))
                    {
                        selectedCommonTreatments.Remove("MRI");
                        mriBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("PT"))
                    {
                        selectedCommonTreatments.Remove("PT");
                        ptBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }

                    if (selectedCommonTreatments.Contains("Injections"))
                    {
                        selectedCommonTreatments.Remove("Injections");
                        injBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    }


                }
            }




        private void CopyURIBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("IB, PC called for AA. Screened; UR");
        }

        private void CopyUROBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("OB Called PC to consult for AA. Screened; UR");
        }

        private void CopyROIBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("IB, PC called for AA. Screened; RO");
        }

        private void CopyROOBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("OB Called PC to consult for AA. Screened; RO");
        }

        private void CopyTDIBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("IB, PC called for AA. Screened; TD. Provided State bar Info.");
        }

        private void CopyTDOBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("OB Called PC to consult for AA. Screened; TD. Provided State bar Info.");
        }

        private void CopySUIBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("IB, PC called for AA. Screened; Sent DocuSign, RR");
        }

        private void CopySUOBPCAA_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText("OB Called PC to consult for AA. Screened; Sent DocuSign, RR");

        }

        private void StateBarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)StateBarComboBox.SelectedItem;
            string stateName = selectedItem.Content.ToString();
            string stateCode = selectedItem.Tag.ToString();

            // get state bar number based on state code
            string stateBarContact = GetstateBarContact(stateCode);

            if (stateBarContact != "N/A")
            {
                // construct message
                string message = $"Hi! This is Morgan & Morgan, here is the State Bar contact for the state of {stateName}: {stateBarContact}. Thank you for calling Morgan & Morgan! We hope you have a speedy recovery, have a great day!";

                // set message text box
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1)
                };
                messageTextBox.BeginAnimation(OpacityProperty, animation);
                messageTextBox.Text = message;
            }
        }

        private string GetstateBarContact(string stateCode)
        {
            // replace with your own logic to retrieve state bar number based on state code
            string stateBarContact = "";

            switch (stateCode)
            {

                case "AL":
                    stateBarContact = "800-354-6154";
                    break;
                case "AK":
                    stateBarContact = "907-272-7469";
                    break;
                case "AZ":
                    stateBarContact = "602-340-7205";
                    break;
                case "AR":
                    stateBarContact = "501-375-4606";
                    break;
                case "CA":
                    stateBarContact = "800-843-9053";
                    break;
                case "CO":
                    stateBarContact = "303-860-1115";
                    break;
                case "CT":
                    stateBarContact = "860-223-4400";
                    break;
                case "DE":
                    stateBarContact = "302-658-5279";
                    break;
                case "FL":
                    stateBarContact = "800-342-8011";
                    break;
                case "GA":
                    stateBarContact = "404-527-8700";
                    break;
                case "HI":
                    stateBarContact = "808-537-1868";
                    break;
                case "ID":
                    stateBarContact = "208-334-4500";
                    break;
                case "IL":
                    stateBarContact = "217-525-1760";
                    break;
                case "IN":
                    stateBarContact = "317-639-5465";
                    break;
                case "IA":
                    stateBarContact = "515-243-3179";
                    break;
                case "KS":
                    stateBarContact = "785-234-5696";
                    break;
                case "KY":
                    stateBarContact = "502-564-3795";
                    break;
                case "LA":
                    stateBarContact = "800-421-5722";
                    break;
                case "ME":
                    stateBarContact = "207-623-1121";
                    break;
                case "MD":
                    stateBarContact = "410-685-7878";
                    break;
                case "MA":
                    stateBarContact = "617-338-0500";
                    break;
                case "MI":
                    stateBarContact = "517-346-6300";
                    break;
                case "MN":
                    stateBarContact = "800-882-6722";
                    break;
                case "MS":
                    stateBarContact = "601-948-4471";
                    break;
                case "MO":
                    stateBarContact = "573-635-4128";
                    break;
                case "MT":
                    stateBarContact = "406-447-2200";
                    break;
                case "NE":
                    stateBarContact = "402-475-7091";
                    break;
                case "NV":
                    stateBarContact = "702-382-2200";
                    break;
                case "NH":
                    stateBarContact = "603-224-6942";
                    break;
                case "NJ":
                    stateBarContact = "800-792-8314";
                    break;
                case "NM":
                    stateBarContact = "505-797-6000";
                    break;
                case "NY":
                    stateBarContact = "800-342-3661";
                    break;
                case "NC":
                    stateBarContact = "919-828-4620";
                    break;
                case "ND":
                    stateBarContact = "800-472-2685";
                    break;
                case "OH":
                    stateBarContact = "800-282-6556";
                    break;
                case "OK":
                    stateBarContact = "405-416-7000";
                    break;
                case "OR":
                    stateBarContact = "503-620-0222";
                    break;
                case "PA":
                    stateBarContact = "800-692-7375";
                    break;
                case "RI":
                    stateBarContact = "401-421-5740";
                    break;
                case "SC":
                    stateBarContact = "803-799-6653";
                    break;
                case "SD":
                    stateBarContact = "605-224-7554";
                    break;
                case "TN":
                    stateBarContact = "615-383-7421";
                    break;
                case "TX":
                    stateBarContact = "800-204-2222";
                    break;
                case "UT":
                    stateBarContact = "801-531-9077";
                    break;
                case "VT":
                    stateBarContact = "802-223-2020";
                    break;
                case "VA":
                    stateBarContact = "804-775-0577";
                    break;
                case "WA":
                    stateBarContact = "800-945-9722";
                    break;
                case "WV":
                    stateBarContact = "304-553-7220";
                    break;
                case "WI":
                    stateBarContact = "608-257-3838";
                    break;
                case "WY":
                    stateBarContact = "307-632-9061";
                    break;
                default:
                    stateBarContact = "N/A";
                    break;
            }

            return stateBarContact;
        }

        private void CopySMS_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(messageTextBox.Text);
        }

        private void policeFaultOP_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void policeFaultPC_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }


        private void startSUTimerButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void InjuryDetailsTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            InjuriesSelections.BeginAnimation(OpacityProperty, fadeInAnimation);
            InjuriesSelections.Visibility = Visibility.Visible;
        }

        private void InjuriesSubmit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => InjuriesSelections.Visibility = Visibility.Collapsed;
            InjuriesSelections.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void TreatmentDetailsTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreatmentSelections.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            TreatmentSelections.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void TreatmentsSubmit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => TreatmentSelections.Visibility = Visibility.Collapsed;
            TreatmentSelections.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void OnSelectedDoctorChanged(object sender, SelectionChangedEventArgs e)
        {

            UpdateTreatments();
        }

        private List<ListBoxItem> filteredItems = new List<ListBoxItem>();


        private void DoctorsSpecialistsTagSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = DoctorsSpecialistsTagSearchBox.Text.ToLower();

            foreach (ListBoxItem item in DoctorsSpecialistsTagListBox.Items)
            {
                if (item.Content.ToString().ToLower().Contains(searchText))
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void IPWasDriver_Checked(object sender, RoutedEventArgs e)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            DriverFUQuestion1.BeginAnimation(StackPanel.OpacityProperty, fadeInAnimation);
            DriverFUQuestions.Visibility = Visibility.Visible;
            DriverFUQuestion1.Visibility = Visibility.Visible;
            PassengerFUQuestion1.Visibility = Visibility.Collapsed;
            DriverFUQuestion2.Visibility = Visibility.Collapsed;
            MassTransitFUQuestion1.Visibility = Visibility.Collapsed;
            UpdateRTB();

            ResetTypeOfAccidentComboBox();

        }
        private void IPWasPassenger_Checked(object sender, RoutedEventArgs e)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            PassengerFUQuestion1.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            PassengerFUQuestion1.Visibility = Visibility.Visible;
            DriverFUQuestion1.Visibility = Visibility.Collapsed;
            DriverFUQuestion2.Visibility = Visibility.Collapsed;
            MassTransitFUQuestion1.Visibility = Visibility.Collapsed;
            UpdateRTB();

            ResetTypeOfAccidentComboBox();
        }

        private void DriverFUQuestion1Yes_Checked(object sender, RoutedEventArgs e)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            DriverFUQuestion2.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            DriverFUQuestion2.Visibility = Visibility.Visible;
            UpdateRTB();

        }

        private void DriverFUQuestion1No_Checked(object sender, RoutedEventArgs e)
        {
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.Stop
            };

            DriverFUQuestion2.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
            DriverFUQuestion2.Visibility = Visibility.Collapsed;
            UpdateRTB();

        }

        private void IPWasMassTransitPassenger_Checked(object sender, RoutedEventArgs e)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            MassTransitFUQuestion1.BeginAnimation(Border.OpacityProperty, fadeInAnimation);
            MassTransitFUQuestion1.Visibility = Visibility.Visible;
            PassengerFUQuestion1.Visibility = Visibility.Collapsed;
            DriverFUQuestion1.Visibility = Visibility.Collapsed;
            DriverFUQuestion2.Visibility = Visibility.Collapsed;
            UpdateRTB();

            ResetTypeOfAccidentComboBox();

            TeslaCheckBox.Visibility = Visibility.Collapsed;
            CarServiceCheckBox.Visibility = Visibility.Collapsed;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            PassengerFUQuestion2.BeginAnimation(Border.OpacityProperty, fadeInAnimation);
            PassengerFUQuestion2.Visibility = Visibility.Visible;
        }

        private void IPWasPedestrian_Checked(object sender, RoutedEventArgs e)
        {
            MassTransitFUQuestion1.Visibility = Visibility.Collapsed;
            PassengerFUQuestion1.Visibility = Visibility.Collapsed;
            DriverFUQuestion1.Visibility = Visibility.Collapsed;
            UpdateRTB();

            TypeOfAccident.Items.Clear();
            TypeOfAccident.Items.Add(new ComboBoxItem { Content = "Pedestrian/Vehicle" });
            TypeOfAccident.SelectedItem = TypeOfAccident.Items[0];

            CarServiceCheckBox.Visibility= Visibility.Collapsed;
            AirbagsCheckBox.Visibility = Visibility.Collapsed;
            TeslaCheckBox.Visibility = Visibility.Collapsed;
            CarExplodedCheckBox.Visibility = Visibility.Collapsed;
            TireExplodedCheckBox.Visibility = Visibility.Collapsed;

        }

        private void IPWasCyclist_Checked(object sender, RoutedEventArgs e)
        {
            MassTransitFUQuestion1.Visibility = Visibility.Collapsed;
            PassengerFUQuestion1.Visibility = Visibility.Collapsed;
            DriverFUQuestion1.Visibility = Visibility.Collapsed;
            UpdateRTB();

            TypeOfAccident.Items.Clear();
            TypeOfAccident.Items.Add(new ComboBoxItem { Content = "Cyclist/Vehicle" });
            TypeOfAccident.SelectedItem = TypeOfAccident.Items[0];

            CarServiceCheckBox.Visibility = Visibility.Collapsed;
            AirbagsCheckBox.Visibility = Visibility.Collapsed;
            TeslaCheckBox.Visibility = Visibility.Collapsed;
            CarExplodedCheckBox.Visibility = Visibility.Collapsed;
            TireExplodedCheckBox.Visibility = Visibility.Collapsed;

        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            PassengerFUQuestion2.Visibility = Visibility.Collapsed;

        }

        private void TypeOfAccident_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeOfAccident.SelectedItem != null)
            {
                UpdateRTB();
            }
        }

        private void ResetTypeOfAccidentComboBox()
        {
            // Restore the original items in the TypeOfAccident combo box
            TypeOfAccident.Items.Clear();
            TypeOfAccident.Items.Add(new ComboBoxItem { Content = "" });
            TypeOfAccident.Items.Add(new ComboBoxItem { Content = "Hit & Run" });
            TypeOfAccident.Items.Add(new ComboBoxItem { Content = "Multi Car" });
            TypeOfAccident.Items.Add(new ComboBoxItem { Content = "Single Vehicle" });
            TypeOfAccident.SelectedItem = TypeOfAccident.Items[0];
        }

        private void StateOfIncident_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRTB();

            if (StateOfIncident.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)StateOfIncident.SelectedItem;
                string selectedState = selectedItem.Content.ToString();

                if (selectedState == "Florida")
                {
                    IPNotTreatedWithin14daysStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    IPNotTreatedWithin14daysStackPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private System.Windows.Controls.Button selectedButton = null;

        private void btnRearEnd_Click(object sender, RoutedEventArgs e)
        {
            AccidentDetailsTextBox.Text = "IP was hit in a rear-end collision, ";
            DoubleAnimation fadeInAnimation1 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation1);
            SetButtonSelected(btnRearEnd);

            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            LikelyScenariosRear_End.BeginAnimation(OpacityProperty, fadeIn);

            LikelyScenariosRear_End.Visibility = Visibility.Visible;
            LikelyScenariosHeadOn.Visibility = Visibility.Collapsed;
            LikelyScenariosTbone.Visibility = Visibility.Collapsed;
            LikelyScenariosSideSwipe.Visibility = Visibility.Collapsed;

        }

        private void btnHeadOn_Click(object sender, RoutedEventArgs e)
        {
            AccidentDetailsTextBox.Text = "IP was hit in a head-on collision, ";
            DoubleAnimation fadeInAnimation2 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation2);
            SetButtonSelected(btnHeadOn);

            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            LikelyScenariosHeadOn.BeginAnimation(OpacityProperty, fadeIn);

            LikelyScenariosRear_End.Visibility = Visibility.Collapsed;
            LikelyScenariosHeadOn.Visibility = Visibility.Visible;
            LikelyScenariosTbone.Visibility = Visibility.Collapsed;
            LikelyScenariosSideSwipe.Visibility = Visibility.Collapsed;

        }

        private void btnTbone_Click(object sender, RoutedEventArgs e)
        {
            AccidentDetailsTextBox.Text = "IP was hit in a T-bone collision, ";
            DoubleAnimation fadeInAnimation3 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation3);
            SetButtonSelected(btnTbone);

            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            LikelyScenariosTbone.BeginAnimation(OpacityProperty, fadeIn);

            LikelyScenariosRear_End.Visibility = Visibility.Collapsed;
            LikelyScenariosHeadOn.Visibility = Visibility.Collapsed;
            LikelyScenariosTbone.Visibility = Visibility.Visible;
            LikelyScenariosSideSwipe.Visibility = Visibility.Collapsed;

        }

        private void btnSideswipe_Click(object sender, RoutedEventArgs e)
        {
            AccidentDetailsTextBox.Text = "IP was hit in a sideswipe collision, ";
            DoubleAnimation fadeInAnimation4 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation4);

            SetButtonSelected(btnSideswipe);

            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            LikelyScenariosSideSwipe.BeginAnimation(OpacityProperty, fadeIn);

            LikelyScenariosRear_End.Visibility = Visibility.Collapsed;
            LikelyScenariosHeadOn.Visibility = Visibility.Collapsed;
            LikelyScenariosTbone.Visibility = Visibility.Collapsed;
            LikelyScenariosSideSwipe.Visibility = Visibility.Visible;
        }

        private void SetButtonSelected(System.Windows.Controls.Button button)
        {
            if (selectedButton != null)
            {
                if (selectedButton == btnRearEnd)
                {
                    selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                }
                else if (selectedButton == btnHeadOn)
                {
                    selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                }
                else if (selectedButton == btnSideswipe)
                {
                    selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                }
                else if (selectedButton == btnTbone)
                {
                    selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
                }

            }

            if (button == btnRearEnd)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6081a1"));
            }
            else if (button == btnHeadOn)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6081a1"));
            }
            else if (button == btnSideswipe)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6081a1"));
            }
            else if (button == btnTbone)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6081a1"));
            }

            selectedButton = button;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // Set the selected button to null
            selectedButton = null;

            // Set the Tag property of each button to "notSelected"
            btnRearEnd.Tag = "notSelected";
            btnHeadOn.Tag = "notSelected";
            btnSideswipe.Tag = "notSelected";
            btnTbone.Tag = "notSelected";

            // Clear the accident details text box
            AccidentDetailsTextBox.Text = "";

            // Hide the likely scenarios panels
            LikelyScenariosRear_End.Visibility = Visibility.Collapsed;
            LikelyScenariosHeadOn.Visibility = Visibility.Collapsed;
            LikelyScenariosSideSwipe.Visibility = Visibility.Collapsed;
            LikelyScenariosTbone.Visibility = Visibility.Collapsed;

            // Set the backgrounds of btnRearEnd and btnHeadOn to their initial colors
            btnRearEnd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
            btnHeadOn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
            btnSideswipe.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
            btnTbone.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));

        }

        private void ApproxDateOfIncident_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void ApproxLawyerSignDate_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void ApproxDateOfIncident_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void btnRearEndScenario3_Click(object sender, RoutedEventArgs e)
        {
            ProofFU.Visibility = Visibility.Visible;
            AccidentDetailsTextBox.Text = "IP rear-ended OP";

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);
        }

        private void btnRearEndScenario1_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioRearEnd = 1;
            UpdateAccidentDetailsRearEnd();
        }

        private void btnRearEndScenario2_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioRearEnd = 2;
            UpdateAccidentDetailsRearEnd();
        }

        private void btnHeadOnScenario3_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioHeadOn = 1;
            UpdateAccidentDetailsHeadOn();
        }

        private void btnHeadOnScenario2_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioHeadOn = 2;
            UpdateAccidentDetailsHeadOn();
        }

        private void btnHeadOnScenario1_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioHeadOn = 3;
            UpdateAccidentDetailsHeadOn();
        }

        private void btnTboneScenario1_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioTbone = 1;
            UpdateAccidentDetailsTbone();

        }

        private void btnTboneScenario2_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioTbone = 2;
            UpdateAccidentDetailsTbone();

        }

        private void btnTboneScenario3_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioTbone = 3;
            UpdateAccidentDetailsTbone();

        }

        private void btnSideSwipeScenario1_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioSideSwipe = 1;
            UpdateAccidentDetailsSideSwipe();

        }

        private void btnSideSwipeScenario2_Click(object sender, RoutedEventArgs e)
        {
            selectedScenarioSideSwipe = 2;
            UpdateAccidentDetailsSideSwipe();

        }




        private int selectedScenarioRearEnd = -1;
        private int selectedScenarioHeadOn = -1;
        private int selectedScenarioTbone = -1;
        private int selectedScenarioSideSwipe = -1;


        private void UpdateAccidentDetailsRearEnd()
        {
            switch (selectedScenarioRearEnd)
            {
                case 1:
                    AccidentDetailsTextBox.Text = "IP was hit in a rear-end collision, IP was sitting at a red light, OP was likely distracted and did not notice.";
                    DoubleAnimation fadeInAnimation1 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation1);
                    break;
                case 2:
                    AccidentDetailsTextBox.Text = "IP was hit in a rear-end collision, IP was driving straight and something/someone came in front of IP, causing them to brake very suddenly, OP did not brake soon enough.";
                    DoubleAnimation fadeInAnimation2 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation2);
                    break;
                default:
                    AccidentDetailsTextBox.Text = "";
                    break;
            }

        }

        private void UpdateAccidentDetailsHeadOn()
        {
            switch (selectedScenarioHeadOn)
            {
                case 1:
                    AccidentDetailsTextBox.Text = "IP was hit in a head-on collision, IP had a green light, OP failed to yield before making a left turn hitting IP.";
                    DoubleAnimation fadeInAnimation1 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation1);
                    break;
                case 2:
                    AccidentDetailsTextBox.Text = "IP was hit in a head-on collision, IP was driving straight, and OP crossed the center line hitting IP.";
                    DoubleAnimation fadeInAnimation2 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation2);
                    break;
                case 3:
                    AccidentDetailsTextBox.Text = "IP was hit in a head-on collision, IP was driving straight, OP got confused and drove on the wrong side of the road hitting IP.";
                    DoubleAnimation fadeInAnimation3 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation3);
                    break;
                default:
                    AccidentDetailsTextBox.Text = "";
                    break;
            }

        }


        private void UpdateAccidentDetailsTbone()
        {
            switch (selectedScenarioTbone)
            {
                case 1:
                    AccidentDetailsTextBox.Text = "IP was hit in a T-bone collision, OP ran a red light and hit IP who had the right of way.";
                    DoubleAnimation fadeInAnimation1 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation1);
                    break;
                case 2:
                    AccidentDetailsTextBox.Text = "IP was hit in a T-bone collision, OP failed to stop at a stop sign and was hit by IP driving straight.";
                    DoubleAnimation fadeInAnimation2 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation2);
                    break;
                case 3:
                    AccidentDetailsTextBox.Text = "IP was hit in a T-bone collision, OP failed to yield at an intersection and hit IP who was going straight.";
                    DoubleAnimation fadeInAnimation3 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation3);
                    break;
                default:
                    AccidentDetailsTextBox.Text = "";
                    break;
            }
        }

        private void UpdateAccidentDetailsSideSwipe()
        {
            switch (selectedScenarioSideSwipe)
            {
                case 1:
                    AccidentDetailsTextBox.Text = "IP was hit in a side-swipe collision, OP attempted to merge into IP's lane without seeing them, causing OP to hit IP.";
                    DoubleAnimation fadeInAnimation1 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation1);
                    break;
                case 2:
                    AccidentDetailsTextBox.Text = "IP was hit in a side-swipe collision, IP was on a two-lane road, OP was coming from the opposite direction and crossed the center line hitting IP.";
                    DoubleAnimation fadeInAnimation2 = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(1),
                    };
                    AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation2);
                    break;
                default:
                    AccidentDetailsTextBox.Text = "";
                    break;
            }
        }


        private void UpdateProofDetails()
        {
            string accidentDetails = "";

            if (witnessCheckBox.IsChecked == true && footageCheckBox.IsChecked == false)
            {
                accidentDetails = "IP rear-ended OP,\nIP has a witness that saw the accident.";
            }
            else if (witnessCheckBox.IsChecked == false && footageCheckBox.IsChecked == true)
            {
                accidentDetails = "IP rear-ended OP,\nIP has footage of the accident.";
            }
            else if (witnessCheckBox.IsChecked == true && footageCheckBox.IsChecked == true)
            {
                accidentDetails = "IP rear-ended OP,\nIP has a witness and footage of the accident.";
            }
            else
            {
                accidentDetails = "IP rear-ended OP,\nIP does not have any witnesses or footage of the accident.";
            }

            AccidentDetailsTextBox.Text = accidentDetails;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AccidentDetailsTextBox.BeginAnimation(System.Windows.Controls.TextBox.OpacityProperty, fadeInAnimation);
        }

        private void CforPC_Checked(object sender, RoutedEventArgs e)
        {

            UpdateRTB();

            TextRange textRange = new TextRange(tbDisplay.Document.ContentStart, tbDisplay.Document.ContentEnd);
            string text = textRange.Text;

            // Replace all occurrences of "IP" with "PC" in the text
            text = text.Replace("IP", "PC");

            tbDisplay.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph(new Run(text));
            tbDisplay.Document.Blocks.Add(paragraph);

            CforPCFU.Visibility = Visibility.Visible;

            PCforIPFU.Visibility = Visibility.Collapsed;
            CforIPFU.Visibility = Visibility.Collapsed;
            checkDoi.Visibility = Visibility.Collapsed;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CforPCFU.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void InjuriesExit_Click(object sender, RoutedEventArgs e)
        {

            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => InjuriesSelections.Visibility = Visibility.Collapsed;
            InjuriesSelections.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        // Clear the items collection before setting the ItemsSource
        private void InjuriesTagSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = InjuriesTagSearchBox.Text.ToLower();

            foreach (ListBoxItem item in InjuriesTagListBox.Items)
            {
                if (item.Content.ToString().ToLower().Contains(searchText))
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void InjuriesTagListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = InjuriesTagListBox.SelectedItems;

            selectedOtherInjuries.Clear();

            foreach (var selectedItem in selectedItems)
            {
                string tag = ((ListBoxItem)selectedItem).Content.ToString();
                selectedOtherInjuries.Add(tag);
            }
            UpdateBrokenBoneDetails();

        }

        private void DeathButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateBrokenBoneDetails();

        }

        private void NoneButton_Checked(object sender, RoutedEventArgs e)
        {
            sInjuryDetails = "At this time, IP has not sustained any injuries related to the accident whatsoever.";
            sInjuryBullets = "\n\nInjuries:\n- None";

            UpdateBrokenBoneDetails();
        }

        private void NoneButton_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateBrokenBoneDetails();
        }

        private void DeathButton_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateBrokenBoneDetails();
        }


        private void witnessCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (noneCheckBox.IsChecked == true)
            {
                noneCheckBox.IsChecked = false;
            }
            UpdateProofDetails();
        }

        private void footageCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (noneCheckBox.IsChecked == true)
            {
                noneCheckBox.IsChecked = false;
            }
            UpdateProofDetails();
        }

        private void noneCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (witnessCheckBox.IsChecked == true)
            {
                witnessCheckBox.IsChecked = false;
            }
            if (footageCheckBox.IsChecked == true)
            {
                footageCheckBox.IsChecked = false;
            }
            UpdateProofDetails();
        }

        private void witnessCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateProofDetails();
        }

        private void footageCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateProofDetails();
        }

        private void noneCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateProofDetails();
        }

        private void TreatmentsExit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeOutAnimation.Completed += (s, _) => TreatmentSelections.Visibility = Visibility.Collapsed;
            TreatmentSelections.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void IPNotTreatedWithin14daysCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTreatments();
        }

        private void NoTreatmentButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTreatments();

            WhyNoTreatment.Visibility = Visibility.Visible;
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            WhyNoTreatment.BeginAnimation(OpacityProperty, fadeInAnimation);
        }



        private void IPNotTreatedWithin14daysCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateTreatments();
        }


        private List<String> selectedCommonTreatments = new List<string>();

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }
                else
                {
                    T found = FindVisualChild<T>(child);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        private void ERbutton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button erButton = (System.Windows.Controls.Button)sender;
            Border erBorder = FindVisualChild<Border>(erButton);

            if (selectedCommonTreatments.Contains("ER"))
            {
                selectedCommonTreatments.Remove("ER");
                erBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }
            else
            {
                selectedCommonTreatments.Add("ER");
                erBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }

            UpdateTreatments();
        }

        private void AmbulanceButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button ambulanceButton = (System.Windows.Controls.Button)sender;
            Border ambulanceBorder = FindVisualChild<Border>(ambulanceButton);

            Border erBorder = FindVisualChild<Border>(ERbutton);

            if (!selectedCommonTreatments.Contains("ER"))
            {
                selectedCommonTreatments.Add("ER");
                erBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }


            if (selectedCommonTreatments.Contains("Ambulance"))
            {
                selectedCommonTreatments.Remove("Ambulance");
                ambulanceBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }
            else
            {
                selectedCommonTreatments.Add("Ambulance");
                ambulanceBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }

            UpdateTreatments();
        }

        private void RXMeds_Click(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Controls.Button rxButton = (System.Windows.Controls.Button)sender;
            Border rxBorder = FindVisualChild<Border>(rxButton);

            if (selectedCommonTreatments.Contains("RX Meds"))
            {
                selectedCommonTreatments.Remove("RX Meds");
            }
            else
            {
                selectedCommonTreatments.Add("RX Meds");
            }

            if (selectedCommonTreatments.Contains("RX Meds"))
            {
                rxBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                rxBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }


        private void UrgentCare_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button urgentCareButton = (System.Windows.Controls.Button)sender;
            Border urgentCareBorder = FindVisualChild<Border>(urgentCareButton);

            if (selectedCommonTreatments.Contains("Urgent Care"))
            {
                selectedCommonTreatments.Remove("Urgent Care");
            }
            else
            {
                selectedCommonTreatments.Add("Urgent Care");
            }

            if (selectedCommonTreatments.Contains("Urgent Care"))
            {
                urgentCareBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                urgentCareBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void OTCs_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button otcButton = (System.Windows.Controls.Button)sender;
            Border otcBorder = FindVisualChild<Border>(otcButton);

            if (selectedCommonTreatments.Contains("OTC Meds"))
            {
                selectedCommonTreatments.Remove("OTC Meds");
            }
            else
            {
                selectedCommonTreatments.Add("OTC Meds");
            }

            if (selectedCommonTreatments.Contains("OTC Meds"))
            {
                otcBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                otcBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }


        private void ChiropractorButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button chiropractorButton = (System.Windows.Controls.Button)sender;
            Border chiropractorBorder = FindVisualChild<Border>(chiropractorButton);

            if (selectedCommonTreatments.Contains("Chiropractor"))
            {
                selectedCommonTreatments.Remove("Chiropractor");
            }
            else
            {
                selectedCommonTreatments.Add("Chiropractor");
            }

            if (selectedCommonTreatments.Contains("Chiropractor"))
            {
                chiropractorBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                chiropractorBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void PrimaryCare_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button primaryCareButton = (System.Windows.Controls.Button)sender;
            Border primaryCareBorder = FindVisualChild<Border>(primaryCareButton);

            if (selectedCommonTreatments.Contains("PrimaryCare"))
            {
                selectedCommonTreatments.Remove("PrimaryCare");
            }
            else
            {
                selectedCommonTreatments.Add("PrimaryCare");
            }

            if (selectedCommonTreatments.Contains("PrimaryCare"))
            {
                primaryCareBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                primaryCareBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void XrayButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button xrayButton = (System.Windows.Controls.Button)sender;
            Border xrayBorder = FindVisualChild<Border>(xrayButton);

            if (selectedCommonTreatments.Contains("Xray"))
            {
                selectedCommonTreatments.Remove("Xray");
            }
            else
            {
                selectedCommonTreatments.Add("Xray");
            }

            if (selectedCommonTreatments.Contains("Xray"))
            {
                xrayBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                xrayBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void CTScanButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button ctScanButton = (System.Windows.Controls.Button)sender;
            Border ctScanBorder = FindVisualChild<Border>(ctScanButton);

            if (selectedCommonTreatments.Contains("CT Scan"))
            {
                selectedCommonTreatments.Remove("CT Scan");
            }
            else
            {
                selectedCommonTreatments.Add("CT Scan");
            }

            if (selectedCommonTreatments.Contains("CT Scan"))
            {
                ctScanBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                ctScanBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }
        private void MRIButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button mriButton = (System.Windows.Controls.Button)sender;
            Border mriBorder = FindVisualChild<Border>(mriButton);

            if (selectedCommonTreatments.Contains("MRI"))
            {
                selectedCommonTreatments.Remove("MRI");
            }
            else
            {
                selectedCommonTreatments.Add("MRI");
            }

            if (selectedCommonTreatments.Contains("MRI"))
            {
                mriBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                mriBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void PhysicalTherapy_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button ptButton = (System.Windows.Controls.Button)sender;
            Border ptBorder = FindVisualChild<Border>(ptButton);

            if (selectedCommonTreatments.Contains("PT"))
            {
                selectedCommonTreatments.Remove("PT");
            }
            else
            {
                selectedCommonTreatments.Add("PT");
            }

            if (selectedCommonTreatments.Contains("PT"))
            {
                ptBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                ptBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void Injections_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button injButton = (System.Windows.Controls.Button)sender;
            Border injBorder = FindVisualChild<Border>(injButton);

            if (selectedCommonTreatments.Contains("Injections"))
            {
                selectedCommonTreatments.Remove("Injections");
            }
            else
            {
                selectedCommonTreatments.Add("Injections");
            }

            if (selectedCommonTreatments.Contains("Injections"))
            {
                injBorder.Background = new SolidColorBrush(Color.FromRgb(96, 129, 161));
            }
            else
            {
                injBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            UpdateTreatments();

        }

        private void CforIP_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

            CforIPFU.Visibility = Visibility.Visible;

            PCforIPFU.Visibility = Visibility.Collapsed;
            CforPCFU.Visibility = Visibility.Collapsed;
            checkDoi.Visibility = Visibility.Collapsed;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CforIPFU.BeginAnimation(OpacityProperty, fadeInAnimation);


        }




        private DispatcherTimer DeadAirTimer;
        private int remainingSeconds = 25;

        private void StartDeadAirTimer()
        {
            DeadAirTimer = new DispatcherTimer();
            DeadAirTimer.Interval = TimeSpan.FromSeconds(1);
            DeadAirTimer.Tick += Timer_Tick;
            DeadAirTimer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;

            if (remainingSeconds <= 5)
            {
                timerBorder.Background = Brushes.DarkRed;
                mediaPlayer.Play();

                while (remainingSeconds <= 5 && mediaPlayer.Position >= mediaPlayer.NaturalDuration)
                {
                    mediaPlayer.Position = TimeSpan.Zero;
                    mediaPlayer.Play();
                    Thread.Sleep(1000); 
                }

            }

            timerText.Text = remainingSeconds.ToString();

            if (remainingSeconds == 0)
            {
                DeadAirTimer.Stop();

                timerBorder.Background = Brushes.Gray;
                ResetTimer();
            }
        }


        private void ResetTimer()
        {
            DeadAirTimer.Stop();
            remainingSeconds = 25;
            timerText.Text = remainingSeconds.ToString();
        }



        private void Animation_Completed(object sender, EventArgs e)
        {
            timerBorder.Background = Brushes.DarkRed;
        }



        private void overlayButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush desiredColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
            SolidColorBrush currentColorBrush = timerBorder.Background as SolidColorBrush;

            if (currentColorBrush != null && currentColorBrush.Color.Equals(desiredColorBrush.Color))
            {
                timerBorder.Background = Brushes.Gray;
                ResetTimer();
            }
            else
            {
                timerBorder.Background = desiredColorBrush;
                StartDeadAirTimer();
            }
        }

        private void CarServiceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UberFUQuestions.Visibility = Visibility.Visible;
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            UberFUQuestions.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void OtherCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OtherInformationStackpanel.Visibility = Visibility.Visible;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            OtherInformationStackpanel.BeginAnimation(OpacityProperty, fadeInAnimation);

        }

        private void CommercialVehicleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Visible;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CommercialVehiclePanel.BeginAnimation(OpacityProperty, fadeInAnimation);

        }

        private void AirbagsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AirbagsMessage.Visibility = Visibility.Visible;
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();


            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            AirbagsMessage.BeginAnimation(OpacityProperty, fadeInAnimation);

        }

        private void TeslaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Visible;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CarAndTireAndTeslaAndDUIMessage.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void CarExplodedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Visible;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CarAndTireAndTeslaAndDUIMessage.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void TireExplodedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Visible;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CarAndTireAndTeslaAndDUIMessage.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void DUIOtherDriverCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Visible;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            CarAndTireAndTeslaAndDUIMessage.BeginAnimation(OpacityProperty, fadeInAnimation);

        }

        private void IPWorkingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IPWorkingPanel.Visibility = Visibility.Visible;
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;
            SettlementOfferPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            IPWorkingPanel.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void SettlementOfferCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SettlementOfferPanel.Visibility = Visibility.Visible;
            IPWorkingPanel.Visibility = Visibility.Collapsed;
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;
            AirbagsMessage.Visibility = Visibility.Collapsed;
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;
            UberFUQuestions.Visibility = Visibility.Collapsed;

            UpdateRTB();

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
            };
            SettlementOfferPanel.BeginAnimation(OpacityProperty, fadeInAnimation);


        }

        private void AirbagsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AirbagsMessage.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void CarServiceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UberFUQuestions.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void IPWorkingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IPWorkingPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void TeslaCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void CarExplodedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void TireExplodedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void DUIOtherDriverCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CarAndTireAndTeslaAndDUIMessage.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void CommercialVehicleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CommercialVehiclePanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void SettlementOfferCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettlementOfferPanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }

        private void OtherCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            OtherInformationStackpanel.Visibility = Visibility.Collapsed;

            UpdateRTB();

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateRTB();

        }

        private void ResetInjuries_Click(object sender, RoutedEventArgs e)
        {
            
            DeathButton.IsChecked = false;
            selectedBones.Clear();
            selectedBodyPains.Clear();
            selectedOtherInjuries.Clear();
            CATnonCAT.Text = string.Empty;
            TagsListBox.SelectedItems.Clear();
            InjuriesTagListBox.SelectedItems.Clear();

            sOtherInjuries = string.Empty;
            sBodyPains = string.Empty;
            sBrokenBones = string.Empty;


            SkullButton.Background = Brushes.White;
            SkullButton.Foreground = Brushes.Black;
            OrbitalButton.Background = Brushes.White;
            OrbitalButton.Foreground = Brushes.Black;
            EyeSocketButton.Background = Brushes.White;
            EyeSocketButton.Foreground = Brushes.Black;
            MandibleButton.Background = Brushes.White;
            MandibleButton.Foreground = Brushes.Black;
            JawButton.Background = Brushes.White;
            JawButton.Foreground = Brushes.Black;
            ClavicleButton.Background = Brushes.White;
            ClavicleButton.Foreground = Brushes.Black;
            CervicelButton.Background = Brushes.White;
            CervicelButton.Foreground = Brushes.Black;
            ThorasicButton.Background = Brushes.White;
            ThorasicButton.Foreground = Brushes.Black;
            LumbarVertebraeButton.Background = Brushes.White;
            LumbarVertebraeButton.Foreground = Brushes.Black;
            ScapulaButton.Background = Brushes.White;
            ScapulaButton.Foreground = Brushes.Black;
            ShoulderButton.Background = Brushes.White;
            ShoulderButton.Foreground = Brushes.Black;
            SternumButton.Background = Brushes.White;
            SternumButton.Foreground = Brushes.Black;
            RibsButton.Background = Brushes.White;
            RibsButton.Foreground = Brushes.Black;
            ElbowButton.Background = Brushes.White;
            ElbowButton.Foreground = Brushes.Black;
            HumerusButton.Background = Brushes.White;
            HumerusButton.Foreground = Brushes.Black;
            RadiusButton.Background = Brushes.White;
            RadiusButton.Foreground = Brushes.Black;
            UlnaButton.Background = Brushes.White;
            UlnaButton.Foreground = Brushes.Black;
            PelvisButton.Background = Brushes.White;
            PelvisButton.Foreground = Brushes.Black;
            SacrumButton.Background = Brushes.White;
            SacrumButton.Foreground = Brushes.Black;
            CoccyxButton.Background = Brushes.White;
            CoccyxButton.Foreground = Brushes.Black;
            CarpalsButton.Background = Brushes.White;
            CarpalsButton.Foreground = Brushes.Black;
            MetacarpalsButton.Background = Brushes.White;
            MetacarpalsButton.Foreground = Brushes.Black;
            PhalangesButton.Background = Brushes.White;
            PhalangesButton.Foreground = Brushes.Black;
            WristButton.Background = Brushes.White;
            WristButton.Foreground = Brushes.Black;
            FemurButton.Background = Brushes.White;
            FemurButton.Foreground = Brushes.Black;
            PatellaButton.Background = Brushes.White;
            PatellaButton.Foreground = Brushes.Black;
            HipButton.Background = Brushes.White;
            HipButton.Foreground = Brushes.Black;
            TibiaButton.Background = Brushes.White;
            TibiaButton.Foreground = Brushes.Black;
            FibulaButton.Background = Brushes.White;
            FibulaButton.Foreground = Brushes.Black;
            AnkleButton.Background = Brushes.White;
            AnkleButton.Foreground = Brushes.Black;
            TarsalsButton.Background = Brushes.White;
            TarsalsButton.Foreground = Brushes.Black;
            MetatarsalsButton.Background = Brushes.White;
            MetatarsalsButton.Foreground = Brushes.Black;
            PhalangesFeetButton.Background = Brushes.White;
            PhalangesFeetButton.Foreground = Brushes.Black;

        }

        private void ResetTreatments_Click(object sender, RoutedEventArgs e)
        {
            UpdateTreatments();
            
            // Find ambulanceBorder
            Border ambulanceBorder = FindVisualChild<Border>(AmbulanceButton);

            // Find ERborder
            Border erBorder = FindVisualChild<Border>(ERbutton);

            // Find urgentCareBorder
            Border urgentCareBorder = FindVisualChild<Border>(UrgentCare);

            // Find xrayBorder
            Border xrayBorder = FindVisualChild<Border>(XrayButton);

            // Find ctScanBorder
            Border ctScanBorder = FindVisualChild<Border>(CTScanButton);

            // Find RXborder
            Border rxBorder = FindVisualChild<Border>(RXMeds);

            // Find otcBorder
            Border otcBorder = FindVisualChild<Border>(OTCs);

            // Find chiropractorBorder
            Border chiropractorBorder = FindVisualChild<Border>(ChiropractorButton);

            // Find primaryCareBorder
            Border primaryCareBorder = FindVisualChild<Border>(PrimaryCare);

            // Find mriBorder
            Border mriBorder = FindVisualChild<Border>(MRIButton);

            // Find ptBorder
            Border ptBorder = FindVisualChild<Border>(PhysicalTherapy);

            // Find injBorder
            Border injBorder = FindVisualChild<Border>(Injections);

            TreatmentDetailsTextBox.Text = "";

            // Clear slDoctorNames list
            slDoctorNames.Clear();

            // Clear selected items in DoctorsSpecialistsTagListBox
            DoctorsSpecialistsTagListBox.SelectedItems.Clear();

            // Uncheck IPNotTreatedWithin14daysCheckBox
            IPNotTreatedWithin14daysCheckBox.IsChecked = false;

            // Clear sTreatmentDetails string
            FinalTreatmentDetails = string.Empty;

            // Clear sTreatmentBullets string
            sTreatmentBullets = string.Empty;



            if (selectedCommonTreatments.Contains("Ambulance"))
            {
                selectedCommonTreatments.Remove("Ambulance");
                ambulanceBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("ER"))
            {
                selectedCommonTreatments.Remove("ER");
                erBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("Urgent Care"))
            {
                selectedCommonTreatments.Remove("Urgent Care");
                urgentCareBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("Xray"))
            {
                selectedCommonTreatments.Remove("Xray");
                xrayBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("CT Scan"))
            {
                selectedCommonTreatments.Remove("CT Scan");
                ctScanBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("RX Meds"))
            {
                selectedCommonTreatments.Remove("RX Meds");
                rxBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("OTC Meds"))
            {
                selectedCommonTreatments.Remove("OTC Meds");
                otcBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("Chiropractor"))
            {
                selectedCommonTreatments.Remove("Chiropractor");
                chiropractorBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("PrimaryCare"))
            {
                selectedCommonTreatments.Remove("PrimaryCare");
                primaryCareBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("MRI"))
            {
                selectedCommonTreatments.Remove("MRI");
                mriBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("PT"))
            {
                selectedCommonTreatments.Remove("PT");
                ptBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }

            if (selectedCommonTreatments.Contains("Injections"))
            {
                selectedCommonTreatments.Remove("Injections");
                injBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
            }
        }

        private void policeFaultPCadmits_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void NoTreatmentsSubmit_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeoutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            fadeoutAnimation.Completed += (s, _) => WhyNoTreatment.Visibility = Visibility.Collapsed;
            WhyNoTreatment.BeginAnimation(OpacityProperty, fadeoutAnimation);
            //WhyNoTreatment.Visibility = Visibility.Collapsed;

            UpdateTreatments();
        }

        private void CommercialVehicleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRTB();
        }

        private void IPWasMassTransitPassenger_Unchecked(object sender, RoutedEventArgs e)
        {
            TeslaCheckBox.Visibility = Visibility.Visible;
            CarServiceCheckBox.Visibility = Visibility.Visible;
        }

        private void DriverinCarService_Checked(object sender, RoutedEventArgs e)
        {
            carServiceCompany.Visibility = Visibility.Visible;
        }

        private void PassengerinCarService_Checked(object sender, RoutedEventArgs e)
        {
            carServiceCompany.Visibility = Visibility.Collapsed;
        }

        private void InjuryDetailsTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InjuriesSelections.Visibility = Visibility.Visible;

        }

        private void IPJobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedComboBoxItem != null && selectedComboBoxItem.Content.ToString() == "Other")
            {
                IPOtherJobPanel.Visibility = Visibility.Visible;
            }
            else
            {
                IPOtherJobPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ApproxLawyerSignDate_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void HavePolice_CopyNo_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }

        private void HavePolice_CopyYes_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRTB();
        }
    }
}


        

