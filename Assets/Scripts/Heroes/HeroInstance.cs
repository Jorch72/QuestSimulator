using Rondo.Generic.Utility;

namespace Rondo.QuestSim.Heroes {

    public class HeroInstance {

        public string DisplayName { get { return GetDisplayName(); } set { m_DisplayName = value; } }
        public string Nickname { get; set; }
        public HeroClasses Class { get; set; }
        public int Experience { get; set; }
        public bool IsDiscovered { get; set; }

        public int Level { get { return (Experience / 20) + 1; } }
        public string ClassProgress { get { return IsDiscovered ? ("Lv" + Level + " " + Class.ToString().ToCamelCase()) : "???"; } }

        private string m_DisplayName;

        public HeroInstance() {

        }

        private string GetDisplayName() {
            if (IsDiscovered) {
                if (string.IsNullOrEmpty(Nickname)) {
                    return m_DisplayName;
                } else {
                    return m_DisplayName.Replace(" ", " \"" + Nickname + "\" ");
                }
            } else {
                return "???";
            }
        }

    }

}