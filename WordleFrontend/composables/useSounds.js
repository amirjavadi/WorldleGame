import { ref, onMounted } from 'vue'

export const useSounds = () => {
  const isSoundEnabled = ref(true)
  const audioContext = ref(null)
  
  const initAudioContext = () => {
    if (typeof window !== 'undefined' && typeof AudioContext !== 'undefined') {
      audioContext.value = new AudioContext()
    }
  }

  const createOscillator = (frequency, duration, type = 'sine') => {
    if (!audioContext.value) return
    
    const oscillator = audioContext.value.createOscillator()
    const gainNode = audioContext.value.createGain()
    
    oscillator.type = type
    oscillator.frequency.value = frequency
    gainNode.gain.value = 0.3
    
    oscillator.connect(gainNode)
    gainNode.connect(audioContext.value.destination)
    
    oscillator.start()
    gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.value.currentTime + duration)
    oscillator.stop(audioContext.value.currentTime + duration)
  }

  const playSound = (soundName) => {
    if (!isSoundEnabled.value || !audioContext.value) return

    switch (soundName) {
      case 'type':
        createOscillator(440, 0.1, 'sine') // A4 note
        break
      case 'error':
        createOscillator(220, 0.3, 'sawtooth') // A3 note with sawtooth wave
        break
      case 'success':
        // Play a simple ascending arpeggio
        setTimeout(() => createOscillator(440, 0.1), 0)   // A4
        setTimeout(() => createOscillator(554.37, 0.1), 100) // C#5
        setTimeout(() => createOscillator(659.25, 0.2), 200) // E5
        break
      case 'gameOver':
        // Play a simple descending arpeggio
        setTimeout(() => createOscillator(440, 0.2), 0)   // A4
        setTimeout(() => createOscillator(349.23, 0.2), 200) // F4
        setTimeout(() => createOscillator(293.66, 0.3), 400) // D4
        break
      case 'backspace':
        createOscillator(330, 0.1, 'sine') // E4 note
        break
    }
  }

  const toggleSound = () => {
    isSoundEnabled.value = !isSoundEnabled.value
    if (typeof window !== 'undefined') {
      localStorage.setItem('soundEnabled', isSoundEnabled.value)
    }
  }

  onMounted(() => {
    if (typeof window !== 'undefined') {
      const savedSoundSetting = localStorage.getItem('soundEnabled')
      if (savedSoundSetting !== null) {
        isSoundEnabled.value = savedSoundSetting === 'true'
      }
      initAudioContext()
    }
  })

  return {
    isSoundEnabled,
    toggleSound,
    playSound
  }
}

export default useSounds 