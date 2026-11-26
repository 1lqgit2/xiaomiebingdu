using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class AudioManager : Singleton<AudioManager>
	{
		private Dictionary<string, AudioClip> _audioClipDic;

		private Dictionary<string, AudioSource> _speciAudioPool;

		private List<AudioSource> _audioBgmList;

		private List<AudioSource> _audioSourceList;

		private GameObject _obj;

		private int _audioSourceCount = 5;

		private int _audioBgmCount = 1;

		private float _volumeBgm = 1f;

		private float _volumeSound = 1f;

		private AudioManager()
		{
			_audioClipDic = new Dictionary<string, AudioClip>();
			_speciAudioPool = new Dictionary<string, AudioSource>();
			_audioSourceList = new List<AudioSource>();
			_audioBgmList = new List<AudioSource>();
		}

		public void Init(params string[] clipNames)
		{
			if (_obj == null)
			{
				_obj = new GameObject("AudioManager");
				_obj.AddComponent<AudioListener>();
			}
			for (int i = 0; i < _audioSourceCount; i++)
			{
				AudioSource item = SpawnAudioSource();
				_audioSourceList.Add(item);
			}
			for (int j = 0; j < _audioBgmCount; j++)
			{
				AudioSource audioSource = SpawnAudioSource();
				audioSource.loop = true;
				_audioBgmList.Add(audioSource);
			}
			if (clipNames != null)
			{
				InitClip(clipNames);
			}
		}

		public void InitClip(string[] clipNames)
		{
			foreach (string text in clipNames)
			{
				AudioClip value = Resources.Load<AudioClip>($"GameMuice/{text}");
				if (!_audioClipDic.ContainsKey(text))
				{
					_audioClipDic.Add(text, value);
				}
			}
		}

		public void PlayBgm(string name)
		{
			AudioSource bgmSource = GetBgmSource();
			if (bgmSource.clip == null || bgmSource.clip.name != name)
			{
				bgmSource.clip = GetAudioClip(name);
			}
			bgmSource.Play();
		}

		public void PlaySpeciSound(string name)
		{
			if (!_speciAudioPool.ContainsKey(name))
			{
				AudioSource value = SpawnAudioSource();
				_speciAudioPool.Add(name, value);
			}
			if (!_speciAudioPool[name].isPlaying)
			{
				AudioClip audioClip = GetAudioClip(name);
				_speciAudioPool[name].volume = _volumeSound;
				_speciAudioPool[name].PlayOneShot(audioClip);
			}
		}

		public void PlaySpeciSound(string name, bool isLoop)
		{
			if (!_speciAudioPool.ContainsKey(name))
			{
				AudioSource value = SpawnAudioSource();
				_speciAudioPool.Add(name, value);
			}
			if (!_speciAudioPool[name].isPlaying)
			{
				AudioClip audioClip = GetAudioClip(name);
				_speciAudioPool[name].volume = _volumeSound;
				_speciAudioPool[name].loop = isLoop;
				_speciAudioPool[name].clip = audioClip;
				_speciAudioPool[name].Play();
			}
		}

		public void StopSpeciSound(string name)
		{
			if (_speciAudioPool.ContainsKey(name) && _speciAudioPool[name].isPlaying)
			{
				_speciAudioPool[name].Stop();
			}
		}

		public void PlaySound(string name)
		{
			PlaySound(name, isReuse: false);
		}

		public void PlaySound(string name, float delay, int count, float offset)
		{
			Timer.Register(delay, delegate
			{
				for (int i = 0; i < count; i++)
				{
					Timer.Register((float)i * offset, delegate
					{
						PlaySound(name);
					});
				}
			});
		}

		public bool IsPlaying(string name)
		{
			for (int i = 0; i < _audioSourceList.Count; i++)
			{
				if (_audioSourceList[i].clip != null && _audioSourceList[i].clip.name == name)
				{
					return _audioSourceList[i].isPlaying;
				}
			}
			return false;
		}

		public void StopSound(string name)
		{
			int num = 0;
			while (true)
			{
				if (num < _audioSourceList.Count)
				{
					if (_audioSourceList[num].clip != null && _audioSourceList[num].clip.name == name)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			_audioSourceList[num].Stop();
		}

		public void PlaySound(string name, bool isReuse = false)
		{
			AudioSource soundSource = GetSoundSource(name);
			if (isReuse)
			{
				if (soundSource.clip == null || soundSource.clip.name != name)
				{
					soundSource.clip = GetAudioClip(name);
				}
			}
			else
			{
				soundSource.clip = GetAudioClip(name);
			}
			soundSource.loop = false;
			soundSource.Play();
		}

		private AudioSource SpawnAudioSource()
		{
			AudioSource audioSource = _obj.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			return audioSource;
		}

		private AudioSource GetBgmSource()
		{
			AudioSource audioSource = null;
			for (int i = 0; i < _audioBgmList.Count; i++)
			{
				if (!_audioBgmList[i].isPlaying)
				{
					audioSource = _audioBgmList[i];
					break;
				}
			}
			if (audioSource == null)
			{
				audioSource = SpawnAudioSource();
			}
			audioSource.volume = _volumeBgm;
			return audioSource;
		}

		private AudioSource GetBgmSource(string name)
		{
			for (int i = 0; i < _audioBgmList.Count; i++)
			{
				AudioClip clip = _audioBgmList[i].clip;
				if (clip != null && clip.name == name)
				{
					return _audioBgmList[i];
				}
			}
			return SpawnAudioSource();
		}

		private AudioSource GetSoundSource(string name)
		{
			AudioSource audioSource = null;
			for (int i = 0; i < _audioSourceList.Count; i++)
			{
				if (!_audioSourceList[i].isPlaying)
				{
					audioSource = _audioSourceList[i];
					break;
				}
			}
			if (audioSource == null)
			{
				audioSource = SpawnAudioSource();
				_audioSourceList.Add(audioSource);
			}
			audioSource.volume = _volumeSound;
			return audioSource;
		}

		private AudioClip GetAudioClip(string name)
		{
			if (_audioClipDic.ContainsKey(name))
			{
				return _audioClipDic[name];
			}
			UnityEngine.Debug.LogError("GetAudioClip name is null" + name);
			return AudioClip.Create(string.Empty, 1, 1, 1, stream: false);
		}

		public void StopAllAudio()
		{
			for (int i = 0; i < _audioBgmList.Count; i++)
			{
				_audioBgmList[i].volume = 0f;
			}
			for (int j = 0; j < _audioSourceList.Count; j++)
			{
				_audioSourceList[j].volume = 0f;
			}
		}

		public void SetAllVolume(float bgm, float sound)
		{
			_volumeBgm = bgm;
			_volumeSound = sound;
			for (int i = 0; i < _audioBgmList.Count; i++)
			{
				_audioBgmList[i].volume = bgm;
			}
			for (int j = 0; j < _audioSourceList.Count; j++)
			{
				_audioSourceList[j].volume = sound;
			}
		}
	}
}
