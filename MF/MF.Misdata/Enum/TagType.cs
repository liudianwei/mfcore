namespace Common.Model
{
    /// <summary>
    /// 11：BOOL 2：INT 3：DINT 17：BYTE 8：STRING 18:WORD 4:REAL
    /// </summary>
    public enum TagType
    {
        /// <summary>
        ///
        /// </summary>
        @short = 2,

        /// <summary>
        ///
        /// </summary>
        shortArray = 2,

        /// <summary>
        ///
        /// </summary>
        @int = 3,

        /// <summary>
        ///
        /// </summary>
        intArray = 3,

        /// <summary>
        ///
        /// </summary>
        @float = 4,

        /// <summary>
        ///
        /// </summary>
        floatArray = 4,

        /// <summary>
        ///
        /// </summary>
        datetime = 7,

        /// <summary>
        ///
        /// </summary>
        datetimeArray = 7,

        /// <summary>
        ///
        /// </summary>
        @string = 8,

        /// <summary>
        ///
        /// </summary>
        @bool = 11,

        /// <summary>
        ///
        /// </summary>
        boolArray = 11,

        /// <summary>
        /// /
        /// </summary>
        @sbyte = 16,

        /// <summary>
        /// /
        /// </summary>
        sbyteArray = 16,

        /// <summary>
        /// /
        /// </summary>
        @byte = 17,

        /// <summary>
        ///
        /// </summary>
        byteArray = 17,

        /// <summary>
        ///
        /// </summary>
        @ushort = 18,

        /// <summary>
        ///
        /// </summary>
        ushortArray = 18,

        /// <summary>
        /// /
        /// </summary>
        @uint = 19,

        /// <summary>
        /// /
        /// </summary>
        uintArray = 19,

        /// <summary>
        ///
        /// </summary>
        time = 2,

        /// <summary>
        ///
        /// </summary>
        timeArray = 2,

        /// <summary>
        ///
        /// </summary>
        tod = 19,

        /// <summary>
        ///
        /// </summary>
        todArray = 19
    }
}