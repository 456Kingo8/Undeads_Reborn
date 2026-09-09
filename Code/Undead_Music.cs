using FMOD;
using FMODUnity;
using System;
using System.IO;

namespace Undeads.Code
{
    internal static class Undead_Music
    {
        private static Sound _sound;
        private static Channel _channel;
        private static string _currentPath;
        private static float _volume = 1f;

        public static RESULT Play(string pPath, bool pLoop = true, float pVolume = 1f)
        {
            Stop();

            if (!RuntimeManager.IsInitialized)
            {
                return RESULT.ERR_UNINITIALIZED;
            }

            string path = Path.GetFullPath(pPath);
            if (!File.Exists(path))
            {
                return RESULT.ERR_FILE_NOTFOUND;
            }

            MODE mode = MODE.CREATESTREAM | MODE._2D;
            mode |= pLoop ? MODE.LOOP_NORMAL : MODE.LOOP_OFF;

            RESULT result = RuntimeManager.CoreSystem.createStream(path, mode, out _sound);
            if (result != RESULT.OK)
            {
                clearSound();
                return result;
            }

            result = RuntimeManager.CoreSystem.playSound(_sound, default(ChannelGroup), true, out _channel);
            if (result != RESULT.OK)
            {
                clearChannel();
                releaseSound();
                return result;
            }

            _volume = clampVolume(pVolume);
            result = _channel.setVolume(_volume);
            if (result == RESULT.OK)
            {
                result = _channel.setPaused(false);
            }
            if (result != RESULT.OK)
            {
                Stop();
                return result;
            }

            _currentPath = path;
            return RESULT.OK;
        }

        public static RESULT PlayFromMod(string pModRoot, string pRelativePath, bool pLoop = true, float pVolume = 1f)
        {
            if (string.IsNullOrWhiteSpace(pModRoot) || string.IsNullOrWhiteSpace(pRelativePath))
            {
                return RESULT.ERR_INVALID_PARAM;
            }

            string root = Path.GetFullPath(pModRoot);
            string path = Path.GetFullPath(Path.Combine(root, pRelativePath));
            string rootPrefix = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!path.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return RESULT.ERR_INVALID_PARAM;
            }
            return Play(path, pLoop, pVolume);
        }

        public static RESULT SetPaused(bool pPaused)
        {
            if (!_channel.hasHandle())
            {
                return RESULT.ERR_INVALID_HANDLE;
            }
            return _channel.setPaused(pPaused);
        }

        public static RESULT SetVolume(float pVolume)
        {
            _volume = clampVolume(pVolume);
            if (!_channel.hasHandle())
            {
                return RESULT.ERR_INVALID_HANDLE;
            }
            return _channel.setVolume(_volume);
        }

        public static bool IsPlaying()
        {
            if (!_channel.hasHandle())
            {
                return false;
            }
            return _channel.isPlaying(out bool playing) == RESULT.OK && playing;
        }

        public static RESULT Stop()
        {
            RESULT result = RESULT.OK;
            if (_channel.hasHandle())
            {
                result = _channel.stop();
                clearChannel();
            }
            if (_sound.hasHandle())
            {
                RESULT releaseResult = _sound.release();
                if (result == RESULT.OK)
                {
                    result = releaseResult;
                }
                clearSound();
            }
            _currentPath = null;
            return result;
        }

        public static string CurrentPath => _currentPath;
        public static float Volume => _volume;

        private static float clampVolume(float pVolume)
        {
            if (pVolume < 0f) return 0f;
            if (pVolume > 1f) return 1f;
            return pVolume;
        }

        private static void releaseSound()
        {
            if (_sound.hasHandle())
            {
                _sound.release();
            }
            clearSound();
        }

        private static void clearChannel()
        {
            if (_channel.hasHandle())
            {
                _channel.clearHandle();
            }
            _channel = default(Channel);
        }

        private static void clearSound()
        {
            if (_sound.hasHandle())
            {
                _sound.clearHandle();
            }
            _sound = default(Sound);
        }
    }
}
