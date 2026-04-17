
using SamanthaDJ.Interface.Model;
using SamanthaX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SamanthaDJ.Interface {

    public static class Samantha {

        #region Properties

        #region SamanthaRun

        /// <summary>
        /// 
        /// </summary>
        public static bool SamanthaRun { get; set; } = false;

        #endregion SamanthaRun

        #region Credentials

        /// <summary>
        /// 
        /// </summary>
        public static string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string Token { get; set; }

        #endregion Credentials

        #region Culture

        /// <summary>
        /// 
        /// </summary>
        public static string CultureInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string UICultureInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string CultureSpeech { get; set; }

        #endregion Culture

        #region Factor

        public static int TickSamanthaListens { get; set; }
        public static int TicksSamAskWhatInstruccion { get; set; }
        public static double SpeechRecognizedConfidence { get; set; }
        public static int TimerInterval { get; set; }

        #endregion Factor

        #region Behavior

        public static string GrammarLoadMode { get; set; }

        #endregion Behavior

        #region SpeechSynthesizer

        public static string SpeechSynthesizerVoice { get; set; }
        public static int SpeechSynthesizerVolume { get; set; }
        public static int SpeechSynthesizerRate { get; set; }

        #endregion SpeechSynthesizer

        #region Log

        /// <summary>
        /// 
        /// </summary>
        public static string PathLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool GenerateParameterLog { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public static bool GenerateRunEventLog { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public static bool GenerateSpeechRecognizedEventLog { get; set; } = true;

        #endregion Log

        #region Samantha Voice

        /// <summary>
        /// 
        /// </summary>
        public static string VoiceProcessingDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string VoiceSolutionDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string VoiceCancelDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string VoiceFailDefault { get; set; }

        #endregion Samantha Voice

        #endregion Properties

        #region Public Events

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventTimeRemaining;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventSamanthaListening;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventSpeechRecognized;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventListenWithoutAttention;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventInstructionArmed;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventDetectedInstructions;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventSpeechDetected;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventAudioStateChange;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventAudioSignalProblem;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler EventSamanthaUnused;

        #endregion Public Events

        #region Variables

        /// <summary>
        /// 
        /// </summary>
        public static SamanthaExecutive SamX { get; set; }

        #endregion Variables

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize() {
            SamX = new SamanthaExecutive();
            SamX.Initialize(
                username: Username,
                token: Token,

                cultureInfo: CultureInfo,
                uiCultureInfo: UICultureInfo,
                cultureSpeech: CultureSpeech,

                tickSamanthaListens: TickSamanthaListens,
                ticksSamAskWhatInstruccion: TicksSamAskWhatInstruccion,
                speechRecognizedConfidence: SpeechRecognizedConfidence,

                timerInterval: TimerInterval,

                grammarLoadMode: GrammarLoadMode,

                speechSynthesizerVoice: SpeechSynthesizerVoice,
                speechSynthesizerVolume: SpeechSynthesizerVolume,
                speechSynthesizerRate: SpeechSynthesizerRate,

                pathlog: PathLog,
                generateLog: GenerateParameterLog,
                generateRunEventLog: GenerateRunEventLog,
                generateSpeechRecognizedEventLog: GenerateSpeechRecognizedEventLog
            ); 
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Run() {

            SamX.Event_Core_TimeRemaining += new EventHandler(EventTimeRemainingFunction);
            SamX.Event_Core_SamanthaListening += new EventHandler(EventSamanthaListeningFunction);
            SamX.Event_Core_SpeechRecognized += new EventHandler(EventSpeechRecognizedFunction);
            SamX.Event_Core_ListenWithoutAttention += new EventHandler(EventListenWithoutAttentionFunction);
            SamX.Event_Core_InstructionArmed += new EventHandler(EventInstructionArmedFunction);
            SamX.Event_Core_DetectedInstructions += new EventHandler(EventDetectedInstructionsFunction);
            SamX.Event_Core_SpeechDetected += new EventHandler(EventSpeechDetectedFunction);
            SamX.Event_Core_AudioStateChange += new EventHandler(EventAudioStateChangeFunction);
            SamX.Event_Core_AudioSignalProblem += new EventHandler(EventAudioSignalProblemFunction);
            SamX.Event_Core_SamanthaUnused += new EventHandler(EventSamanthaUnusedFunction);

            VoiceProcessingDefault = SamX.VoiceProcessingDefault;
            VoiceSolutionDefault = SamX.VoiceSolutionDefault;
            VoiceCancelDefault = SamX.VoiceCancelDefault;
            VoiceFailDefault = SamX.VoiceFailDefault;

            try {
                SamX.Run();
            } catch (Exception ex) {
                //WriteLogFile("SamanthaRun", $"Error ocurred SamanthaExecutive.Run: {ex.Message}\nStackTrace: {ex.StackTrace}");
                //SamanthaRun = true;
                throw new Exception(ex.Message);
            }
            //SamanthaRun = true;
        }

        #endregion Public Methods

        #region Call Back Event

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventTimeRemainingFunction(object sender, EventArgs e) {
            EventTimeRemaining(sender, e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventSamanthaListeningFunction(object sender, EventArgs e) {
            EventSamanthaListening(sender,e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventSpeechRecognizedFunction(object sender, EventArgs e) {
            EventSpeechRecognized(sender, e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventListenWithoutAttentionFunction(object sender, EventArgs e) {
            EventListenWithoutAttention(sender, e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventInstructionArmedFunction(object sender, EventArgs e) {
            EventInstructionArmed(sender,e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventDetectedInstructionsFunction(object sender, EventArgs e) {
            EventDetectedInstructions(sender, e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventSpeechDetectedFunction(object sender, EventArgs e) {
            string data = (string)sender;
            EventSpeechDetected(data, e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventAudioStateChangeFunction(object sender, EventArgs e) {
            string data = (string)sender;
            EventAudioStateChange(data, e);
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventAudioSignalProblemFunction(object sender, EventArgs e) {
            try {
                EventAudioSignalProblem(sender,e);
            } catch {
            }
        }

        /// <summary>
        ///     ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventSamanthaUnusedFunction(object sender, EventArgs e) {
            EventSamanthaUnused(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private static void EventTroubleInitializeFunction(object sender, EventArgs e) {
        //    EventTroubleInitialize(sender, e);
        //}

        #endregion Call Back Event

        #region Interface Function

        /// <summary>
        /// 
        /// </summary>
        public static bool ReconnectAudio() {
            return SamX.ReconectAudio();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public static void SpeechText(string text) {
            SamX.SpeechText(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instruction"></param>
        public static void SpeechInstruction(SamanthaInstruction instruction) {
            SamX.SpeechText(
                string.IsNullOrEmpty(instruction.VoiceProcessing)
                ? VoiceProcessingDefault
                : instruction.VoiceProcessing
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSentence"></param>
        public static void GetTextInstruction(string strSentence) {
            SamX.GetTextInstruction(strSentence);
        }

        #endregion Interface Function

        #region Utility

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="strInfo"></param>
        /// <param name="hour"></param>
        public static void WriteLogFile(string fileName, string strInfo, bool hour = false) {
            SamX.WriteLogFile(fileName,strInfo,hour);
        }

        #endregion Utility

        #region Registered Event - Obsolte

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventTimeRemainingRegistered(Delegate eventAnalyzed) {
            if (EventTimeRemaining != null) {
                foreach (Delegate itemLoaded in EventTimeRemaining.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventListeningRegistered(Delegate eventAnalyzed) {
            if (EventListenWithoutAttention != null) {
                foreach (Delegate itemLoaded in EventListenWithoutAttention.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventSamanthaListeningRegistered(Delegate eventAnalyzed) {
            if (EventSamanthaListening != null) {
                foreach (Delegate itemLoaded in EventSamanthaListening.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventWordRecognizedRegistered(Delegate eventAnalyzed) {
            if (EventSpeechDetected != null) {
                foreach (Delegate itemLoaded in EventSpeechDetected.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventInstructionArmedRegistered(Delegate eventAnalyzed) {
            if (EventInstructionArmed != null) {
                foreach (Delegate itemLoaded in EventInstructionArmed.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventAudioSignalProblemRegistered(Delegate eventAnalyzed) {
            if (EventAudioSignalProblem != null) {
                foreach (Delegate itemLoaded in EventAudioSignalProblem.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion Registered Event - Obsolete

        #region Registered Event Net 10 Core Blazor - Obsolete

        /// <summary>
        ///     Version Net 10 Core Blazor
        /// </summary>
        /// <param name="eventAnalyzed"></param>
        /// <returns></returns>
        public static bool IsEventDetectedInstructionsRegistered(Delegate eventAnalyzed) {
            if (EventDetectedInstructions != null) {
                foreach (Delegate itemLoaded in EventDetectedInstructions.GetInvocationList()) {
                    if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     Net Framework 4.8 Version
        /// </summary>
        /// <param name="sC_TimeRemainingFunction"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsEventDetectedInstructionsRegistered48(Action<object, EventArgs> sC_TimeRemainingFunction) {
            //throw new NotImplementedException();
            if (EventDetectedInstructions != null) {
                foreach (Delegate itemLoaded in EventDetectedInstructions.GetInvocationList()) {
                    //if (itemLoaded.Method.Name == eventAnalyzed.Method.Name) {
                    //    return true;
                    //}
                }
            }
            return false;
        }

        #endregion Registered Event Net 10 Core Blazor - Obsolete
    
    }
}
