using KeySeq.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySeq.Models
{
    public class Sequence
    {
        /// <summary>
        /// Get or Set Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Get or Set keyToPress
        /// </summary>
        public char KeyToPress { get; set; }

        /// <summary>
        /// Get or Set DurationKeyToPress
        /// </summary>
        public int DurationKeyToPress { get; set; }

        /// <summary>
        /// Get or Set DirectionToFace
        /// </summary>
        public Direction DirectionToFace { get; set; }

        public Sequence()
        {
            Enabled = true;
            KeyToPress = ' ';
            DurationKeyToPress = 100;
            DirectionToFace = Direction.Right;
        }
    }
}
