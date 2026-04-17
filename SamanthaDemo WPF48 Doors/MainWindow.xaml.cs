
using SamanthaDJ.Interface;
using SamanthaDJ.Interface.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SamanthaDemo_WPF48_Doors {

    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        #region Variables

        private SerialPort _serialPort = new SerialPort();
        private double _ticksSamanthaListen = 35;

        #endregion Variables

        #region Constructors

        public MainWindow() {
            InitializeComponent();

            this.ProgressBarListening.Minimum = 0;
            this.ProgressBarListening.Maximum = _ticksSamanthaListen;
            this.ProgressBarListening.Visibility = Visibility.Hidden;
            this.TextBlockListening.Visibility = Visibility.Hidden;
        }

        #endregion Constructors

        #region Window Events

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            try {
                #region Samantha

                if (!Samantha.SamanthaRun) {

                    Samantha.Username = "MyEMail@MyDomain.com";
                    Samantha.Token = "ThisIsA60CharTokenDemonstrationPurposes=>DavidJaguarSoft.com";

                    Samantha.CultureInfo = "es-ES";
                    Samantha.UICultureInfo = "es-ES";
                    Samantha.CultureSpeech = "es-MX";

                    Samantha.TickSamanthaListens = Convert.ToInt32(_ticksSamanthaListen);
                    Samantha.TicksSamAskWhatInstruccion = 10;
                    Samantha.SpeechRecognizedConfidence = 0.5;
                    Samantha.TimerInterval = 100;

                    Samantha.GrammarLoadMode = "INSTRUCTIONONLY";

                    Samantha.SpeechSynthesizerVoice = "Microsoft Helena Desktop";
                    Samantha.SpeechSynthesizerVolume = 95;
                    Samantha.SpeechSynthesizerRate = -1;

                    Samantha.PathLog = "C:\\DavidJaguarSoft\\SamanthaDJ\\LogFile";
                    Samantha.GenerateParameterLog = true;
                    Samantha.GenerateRunEventLog = true;
                    Samantha.GenerateSpeechRecognizedEventLog = true;

                    Samantha.Initialize();

                    Samantha.EventTimeRemaining += new EventHandler(SC_TimeRemainingFunction);
                    Samantha.EventSamanthaListening += new EventHandler(SC_SamanthaListeningFunction);
                    Samantha.EventSpeechRecognized += new EventHandler(SC_SpeechRecognizedFunction);
                    Samantha.EventListenWithoutAttention += new EventHandler(SC_ListenWithoutAttentionFunction);
                    Samantha.EventInstructionArmed += new EventHandler(SC_InstructionArmedFunction);
                    Samantha.EventDetectedInstructions += new EventHandler(SC_EventDetectedInstructions);
                    Samantha.EventSpeechDetected += new EventHandler(SC_SpeechDetectedFunction);
                    Samantha.EventAudioStateChange += new EventHandler(SC_EventAudioStateChange);
                    Samantha.EventAudioSignalProblem += new EventHandler(SC_EventAudioSignalProblem);
                    Samantha.EventSamanthaUnused += new EventHandler(SC_EventSamanthaUnused);

                    Samantha.Run();

                    Samantha.SamanthaRun = true;
                    Samantha.SpeechText("Samantha en línea y en espera de instrucciones");
                }

                #endregion Samantha

                #region Serial Port

                _serialPort = new SerialPort();
                _serialPort.PortName = "COM5";
                _serialPort.BaudRate = 9600;
                _serialPort.Open();

                #endregion Serial Port

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show($"An Error ocurred {ex.Message}");
            }
        }

        #region Hyperlink_RequestNavigate

        /// <summary>
        ///     Send to Samantha Web's website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            // Obtiene la URI del enlace
            string uri = e.Uri.AbsoluteUri;

            // Inicia el proceso del navegador predeterminado
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });

            // Marca el evento como manejado para evitar navegación interna
            e.Handled = true;
        }

        #endregion Hyperlink_RequestNavigate

        #region Button Events

        private void ButtonDoor_Click(object sender, RoutedEventArgs e) {

            System.Windows.Controls.Button buttonAux = (System.Windows.Controls.Button)sender;
            string idButton = buttonAux.Name;
            string idLabelButton = string.Empty;
            string idRotationButton = string.Empty;
            string arduinoOpenGate = string.Empty;
            string arduinoCloseGate = string.Empty;
            switch (idButton) {
                case "ButtonDoorOne":
                    idLabelButton = "uno";
                    idRotationButton = "ButtonDoorOneRotation";
                    arduinoOpenGate = "a";
                    arduinoCloseGate = "1";
                    break;
                case "ButtonDoorTwo":
                    idLabelButton = "dos";
                    idRotationButton = "ButtonDoorTowRotation";
                    arduinoOpenGate = "b";
                    arduinoCloseGate = "2";
                    break;
                case "ButtonDoorThree":
                    idLabelButton = "tres";
                    idRotationButton = "ButtonDoorThreeRotation";
                    arduinoOpenGate = "c";
                    arduinoCloseGate = "3";
                    break;
                case "ButtonDoorFour":
                    idLabelButton = "cuatro";
                    idRotationButton = "ButtonDoorFourRotation";
                    arduinoOpenGate = "d";
                    arduinoCloseGate = "4";
                    break;
                default:
                    break;
            }
            System.Windows.Controls.Button buttonControl = this.FindName(idButton) as System.Windows.Controls.Button;
            if (buttonControl.Background.Equals(Brushes.Blue)) {
                //  If open? then close
                DoorMove(
                    idButton: idButton,
                    idRotation: idRotationButton,
                    open: false,
                    actionSpeech: $"Cerrando la puerta {idLabelButton}",
                    from: 90,
                    to: 0,
                    arduinoCode: arduinoCloseGate,
                    brush: Brushes.Red
                );
            } else {
                //  else then open
                DoorMove(
                    idButton: idButton,
                    idRotation: idRotationButton,
                    open: true,
                    actionSpeech: $"Abriendo la puerta {idLabelButton}",
                    from: 0,
                    to: 90,
                    arduinoCode: arduinoOpenGate,
                    brush: Brushes.Blue
                );
            }
        }

        private void ButtonOpenAll_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(
                "Are you sure you want to OPEN all the doors ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == System.Windows.Forms.DialogResult.Yes) {
                Samantha.SpeechText("Abriendo todas las puertas");

                DoorMove(
                   idButton: "ButtonDoorOne", "ButtonDoorOneRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 0, 90,
                   arduinoCode: "a",
                   brush: Brushes.Blue
                );
                DoorMove(
                   idButton: "ButtonDoorTwo", "ButtonDoorTowRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 0, 90,
                   arduinoCode: "b",
                   brush: Brushes.Blue
                );
                DoorMove(
                   idButton: "ButtonDoorThree", "ButtonDoorThreeRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 0, 90,
                   arduinoCode: "c",
                   brush: Brushes.Blue
                );
                DoorMove(
                   idButton: "ButtonDoorFour", "ButtonDoorFourRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 0, 90,
                   arduinoCode: "d",
                   brush: Brushes.Blue
                );
            }
        }

        private void ButtonCloseAll_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(
                "Are you sure you want to CLOSE all the doors ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == System.Windows.Forms.DialogResult.Yes) {
                Samantha.SpeechText("Cerrando todas las puertas");

                DoorMove(
                   idButton: "ButtonDoorOne", "ButtonDoorOneRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 90, 0,
                   arduinoCode: "1",
                   brush: Brushes.Red
                );
                DoorMove(
                   idButton: "ButtonDoorTwo", "ButtonDoorTowRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 90, 0,
                   arduinoCode: "2",
                   brush: Brushes.Red
                );
                DoorMove(
                   idButton: "ButtonDoorThree", "ButtonDoorThreeRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 90, 0,
                   arduinoCode: "3",
                   brush: Brushes.Red
                );
                DoorMove(
                   idButton: "ButtonDoorFour", "ButtonDoorFourRotation",
                   open: true,
                   actionSpeech: string.Empty,
                   from: 90, 0,
                   arduinoCode: "4",
                   brush: Brushes.Red
                );
            }
        }

        #endregion Button Events

        #endregion Window Events

        #region Samantha Event Handlers

        private void SC_TimeRemainingFunction(object sender, EventArgs e) {
            long aux = (long)sender;

            this.Dispatcher.Invoke(() => {
                this.ProgressBarListening.Value = aux;
                this.ProgressBarListening.Visibility = aux > 0 ? Visibility.Visible : Visibility.Hidden;
                this.TextBlockListening.Visibility = aux > 0 ? Visibility.Visible : Visibility.Hidden;
            });
        }

        private async void SC_SamanthaListeningFunction(object sender, EventArgs e) {
        }

        private async void SC_SpeechRecognizedFunction(object sender, EventArgs e) {
            string sentence = sender as string;
            this.Dispatcher.Invoke(() => {
                this.TextBlockInstructionDetected.Text = sentence;
            });
        }

        private async void SC_ListenWithoutAttentionFunction(object sender, EventArgs e) {
        }

        private async void SC_InstructionArmedFunction(object sender, EventArgs e) {
        }

        #region SC_EventDetectedInstructions

        private async void SC_EventDetectedInstructions(object sender, EventArgs e) {
            SamanthaInstructionResponse oInstructions = Newtonsoft.Json.JsonConvert
               .DeserializeObject<SamanthaInstructionResponse>((string)sender);
            if (oInstructions.RecognizedInstructionList != null && oInstructions.RecognizedInstructionList.Count > 0) {
                SamanthaInstruction oSI = oInstructions.RecognizedInstructionList[0];
                this.Dispatcher.Invoke(() => {

                    switch (oSI.InstructionCode) {
                        case "ABRIR_PUERTA_UNO":
                            if (this.ButtonDoorOne.Background.Equals(Brushes.Blue)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta abierta");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorOne",
                                    idRotation: "ButtonDoorOneRotation",
                                    open: true,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 0,
                                    to: 90,
                                    arduinoCode: "a",
                                    brush: Brushes.Blue
                                );
                            }
                            break;
                        case "ABRIR_PUERTA_DOS":
                            if (this.ButtonDoorTwo.Background.Equals(Brushes.Blue)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta abierta");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorTwo",
                                    idRotation: "ButtonDoorTowRotation",
                                    open: true,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 0,
                                    to: 90,
                                    arduinoCode: "b",
                                    brush: Brushes.Blue
                                );
                            }
                            break;
                        case "ABRIR_PUERTA_TRES":
                            if (this.ButtonDoorThree.Background.Equals(Brushes.Blue)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta abierta");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorThree",
                                    idRotation: "ButtonDoorThreeRotation",
                                    open: true,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 0,
                                    to: 90,
                                    arduinoCode: "c",
                                    brush: Brushes.Blue
                                );
                            }
                            break;
                        case "ABRIR_PUERTA_CUATRO":
                            if (this.ButtonDoorFour.Background.Equals(Brushes.Blue)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta abierta");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorFour",
                                    idRotation: "ButtonDoorFourRotation",
                                    open: true,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 0,
                                    to: 90,
                                    arduinoCode: "d",
                                    brush: Brushes.Blue
                                );
                            }
                            break;

                        case "ABRIR_TODO_PUERTA":
                            Samantha.SpeechText(oSI.VoiceProcessing);
                            DoorMove(
                               idButton: "ButtonDoorOne", "ButtonDoorOneRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 0, 90,
                               arduinoCode: "a",
                               brush: Brushes.Blue
                            );
                            DoorMove(
                               idButton: "ButtonDoorTwo", "ButtonDoorTowRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 0, 90,
                               arduinoCode: "b",
                               brush: Brushes.Blue
                            );
                            DoorMove(
                               idButton: "ButtonDoorThree", "ButtonDoorThreeRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 0, 90,
                               arduinoCode: "c",
                               brush: Brushes.Blue
                            );
                            DoorMove(
                               idButton: "ButtonDoorFour", "ButtonDoorFourRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 0, 90,
                               arduinoCode: "d",
                               brush: Brushes.Blue
                            );
                            break;

                        case "CERRAR_PUERTA_UNO":
                            if (this.ButtonDoorOne.Background.Equals(Brushes.Red)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta Cerrada");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorOne",
                                    idRotation: "ButtonDoorOneRotation",
                                    open: false,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 90,
                                    to: 0,
                                    arduinoCode: "1",
                                    brush: Brushes.Red
                                );
                            }
                            break;
                        case "CERRAR_PUERTA_DOS":
                            if (this.ButtonDoorTwo.Background.Equals(Brushes.Red)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta Cerrada");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorTwo",
                                    idRotation: "ButtonDoorTowRotation",
                                    open: false,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 90,
                                    to: 0,
                                    arduinoCode: "2",
                                    brush: Brushes.Red
                                );
                            }
                            break;
                        case "CERRAR_PUERTA_TRES":
                            if (this.ButtonDoorThree.Background.Equals(Brushes.Red)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta Cerrada");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorThree",
                                    idRotation: "ButtonDoorThreeRotation",
                                    open: false,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 90,
                                    to: 0,
                                    arduinoCode: "3",
                                    brush: Brushes.Red
                                );
                            }
                            break;
                        case "CERRAR_PUERTA_CUATRO":
                            if (this.ButtonDoorFour.Background.Equals(Brushes.Red)) {
                                //  If the door is open ? then I can´t open the door
                                Samantha.SpeechText("La puerta ya esta Cerrada");
                            } else {
                                //  else then open
                                DoorMove(
                                    idButton: "ButtonDoorFour",
                                    idRotation: "ButtonDoorFourRotation",
                                    open: false,
                                    actionSpeech: oSI.VoiceProcessing,
                                    from: 90,
                                    to: 0,
                                    arduinoCode: "4",
                                    brush: Brushes.Red
                                );
                            }
                            break;

                        case "CERRAR_TODO_PUERTA":
                            Samantha.SpeechText(oSI.VoiceProcessing);
                            DoorMove(
                               idButton: "ButtonDoorOne", "ButtonDoorOneRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 90, 0,
                               arduinoCode: "1",
                               brush: Brushes.Red
                            );
                            DoorMove(
                               idButton: "ButtonDoorTwo", "ButtonDoorTowRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 90, 0,
                               arduinoCode: "2",
                               brush: Brushes.Red
                            );
                            DoorMove(
                               idButton: "ButtonDoorThree", "ButtonDoorThreeRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 90, 0,
                               arduinoCode: "3",
                               brush: Brushes.Red
                            );
                            DoorMove(
                               idButton: "ButtonDoorFour", "ButtonDoorFourRotation",
                               open: true,
                               actionSpeech: string.Empty,
                               from: 90, 0,
                               arduinoCode: "4",
                               brush: Brushes.Red
                            );
                            break;
                        default:
                            Samantha.SpeechText("Instruccioón no válida");
                            break;
                    }
                });
            }
        }

        #endregion SC_EventDetectedInstructions

        private async void SC_SpeechDetectedFunction(object sender, EventArgs e) {
            string data = (string)sender;
        }

        private async void SC_EventAudioStateChange(object sender, EventArgs e) {
            string data = (string)sender;
        }

        private async void SC_EventAudioSignalProblem(object sender, EventArgs e) {
        }

        private async void SC_EventSamanthaUnused(object sender, EventArgs e) {
            this.Dispatcher.Invoke(() => {
                Samantha.SpeechText("Sin instrucción");
            });
        }

        private async void SC_EventTroubleInitialize(object sender, EventArgs e) {
            string message = (string)sender;
        }

        #endregion Samantha Event Handlers

        #region Methods

        private void DoorMove(
            string idButton,
            string idRotation,
            bool open,
            string actionSpeech,
            double from,
            double to,
            string arduinoCode,
            SolidColorBrush brush
        ) {
            Samantha.SpeechText(actionSpeech);

            try {
                _serialPort.Write($"{arduinoCode}\"");
            } catch{

            }

            DoubleAnimation animation = new DoubleAnimation();
            animation = new DoubleAnimation {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(1.3)),
            };
            string strButton = idButton;
            System.Windows.Controls.Button buttonControl = this.FindName(strButton) as System.Windows.Controls.Button;
            buttonControl.Background = brush;

            string strRotationButton = idRotation;
            RotateTransform rotationButtonControl = this.FindName(strRotationButton) as RotateTransform;
            rotationButtonControl.BeginAnimation(RotateTransform.AngleProperty, animation);

            buttonControl.Background = brush;
        }

        #endregion Methods
    }
}
