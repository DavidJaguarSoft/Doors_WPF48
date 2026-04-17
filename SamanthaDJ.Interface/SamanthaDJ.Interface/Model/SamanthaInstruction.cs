using System.Collections.Generic;

namespace SamanthaDJ.Interface.Model {

    public class SamanthaInstructionResponse {
        public List<SamanthaInstruction> RecognizedInstructionList { get; set; }
        public List<SamanthaInstruction> UnrecognizedInstructionList { get; set; }

        public SamanthaInstructionResponse() {
            RecognizedInstructionList = new List<SamanthaInstruction>();
            UnrecognizedInstructionList = new List<SamanthaInstruction>();
        }
    }

    public class SamanthaInstruction {
        public int InstructionId { get; set; }
        public int InstructionTypeId { get; set; }
        public string Grammar { get; set; }
        public string Sentence { get; set; }
        public float Confidence { get; set; }
        public string InstructionCode { get; set; }
        public string Instruction { get; set; }
        public string Description { get; set; }
        public string VoiceProcessing { get; set; }
        public string VoiceSolution { get; set; }
        public string VoiceCancel { get; set; }
        public string VoiceFail { get; set; }
        public bool InstructionFound { get; set; }

        public SamanthaInstruction() {
            InstructionId = 0;
            InstructionTypeId = 0;
            Grammar = string.Empty;
            Sentence = string.Empty;
            Confidence = 0;
            InstructionCode = string.Empty;
            Instruction = string.Empty;
            Description = string.Empty;
            VoiceProcessing = string.Empty;
            VoiceSolution = string.Empty;
            VoiceCancel = string.Empty;
            VoiceFail = string.Empty;
            InstructionFound = false;
        }
    }
}
