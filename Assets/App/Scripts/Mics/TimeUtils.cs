public static class TimeUtils {
    static float SIMULATE_DELTA_TIME = 1f / 60;
    static int   FRAME_COUNT         = 0;

    public static float deltaTime {
        get {
        #if AI_TOOL && AI_FF
            return SIMULATE_DELTA_TIME;
        #else
            return UnityEngine.Time.deltaTime;
        #endif
        }
    }

    public static int frameCount {
        get {
        #if AI_TOOL && AI_FF
            return FRAME_COUNT;
        #else
            return UnityEngine.Time.frameCount;
        #endif
        }
    }

    public static void IncreaseFrameCount() {
        FRAME_COUNT += 1;
    }
    
    public static void ResetFrameCount() {
        FRAME_COUNT = 0;
    }
}