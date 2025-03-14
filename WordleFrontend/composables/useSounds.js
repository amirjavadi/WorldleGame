import { ref, onMounted } from 'vue'

export const useSounds = () => {
  const isSoundEnabled = ref(true)
  const audioContext = ref(null)
  
  const initAudioContext = () => {
    if (process.client && !audioContext.value) {
      audioContext.value = new (window.AudioContext || window.webkitAudioContext)()
    }
  }

  const createOscillator = (type, frequency, duration, time = 0) => {
    const oscillator = audioContext.value.createOscillator()
    const gainNode = audioContext.value.createGain()
    
    oscillator.type = type
    oscillator.frequency.setValueAtTime(frequency, audioContext.value.currentTime + time)
    
    gainNode.gain.setValueAtTime(0.00001, audioContext.value.currentTime + time)
    gainNode.gain.exponentialRampToValueAtTime(0.3, audioContext.value.currentTime + time + 0.01)
    gainNode.gain.exponentialRampToValueAtTime(0.00001, audioContext.value.currentTime + time + duration)
    
    oscillator.connect(gainNode)
    gainNode.connect(audioContext.value.destination)
    
    return oscillator
  }

  const playSound = (type) => {
    if (!isSoundEnabled.value || !process.client) return
    
    initAudioContext()
    
    switch (type) {
      case 'type':
        // صدای تایپ: یک بیپ کوتاه و ملایم
        const typeOsc = createOscillator('sine', 440, 0.1)
        typeOsc.start(audioContext.value.currentTime)
        typeOsc.stop(audioContext.value.currentTime + 0.1)
        break
        
      case 'error':
        // صدای خطا: دو نت پایین پشت سر هم
        const errorOsc1 = createOscillator('sawtooth', 220, 0.15)
        const errorOsc2 = createOscillator('sawtooth', 196, 0.15, 0.15)
        errorOsc1.start(audioContext.value.currentTime)
        errorOsc1.stop(audioContext.value.currentTime + 0.15)
        errorOsc2.start(audioContext.value.currentTime + 0.15)
        errorOsc2.stop(audioContext.value.currentTime + 0.3)
        break
        
      case 'success':
        // صدای موفقیت: آرپژ صعودی با 4 نت
        const notes = [261.63, 329.63, 392.00, 523.25] // C4, E4, G4, C5
        notes.forEach((freq, i) => {
          const successOsc = createOscillator('sine', freq, 0.15, i * 0.15)
          successOsc.start(audioContext.value.currentTime + i * 0.15)
          successOsc.stop(audioContext.value.currentTime + (i * 0.15) + 0.15)
        })
        break
        
      case 'gameOver':
        // صدای باخت: ترکیب چند صدا برای ایجاد افکت شکست
        const baseFreq = 150
        for (let i = 0; i < 8; i++) {
          const freq = baseFreq - (i * 10)
          const loseOsc = createOscillator('sawtooth', freq, 0.2, i * 0.1)
          loseOsc.start(audioContext.value.currentTime + i * 0.1)
          loseOsc.stop(audioContext.value.currentTime + (i * 0.1) + 0.2)
        }
        // اضافه کردن یک صدای بم در پس‌زمینه
        const backgroundOsc = createOscillator('sine', 80, 1)
        backgroundOsc.start(audioContext.value.currentTime)
        backgroundOsc.stop(audioContext.value.currentTime + 1)
        break
        
      case 'win':
        // صدای برد: ترکیبی از چند نت شاد و فانفار
        const winNotes = [
          { freq: 523.25, time: 0, duration: 0.2 },    // C5
          { freq: 659.25, time: 0.2, duration: 0.2 },  // E5
          { freq: 783.99, time: 0.4, duration: 0.2 },  // G5
          { freq: 1046.50, time: 0.6, duration: 0.4 }, // C6
          { freq: 987.77, time: 1.0, duration: 0.3 },  // B5
          { freq: 1046.50, time: 1.3, duration: 0.5 }  // C6
        ]
        
        winNotes.forEach(note => {
          const winOsc = createOscillator('sine', note.freq, note.duration, note.time)
          winOsc.start(audioContext.value.currentTime + note.time)
          winOsc.stop(audioContext.value.currentTime + note.time + note.duration)
          
          // اضافه کردن هارمونی
          const harmonyOsc = createOscillator('triangle', note.freq * 1.5, note.duration, note.time)
          harmonyOsc.start(audioContext.value.currentTime + note.time)
          harmonyOsc.stop(audioContext.value.currentTime + note.time + note.duration)
        })
        break
        
      case 'backspace':
        // صدای پاک کردن: یک بیپ کوتاه با فرکانس پایین
        const backspaceOsc = createOscillator('sine', 220, 0.08)
        backspaceOsc.start(audioContext.value.currentTime)
        backspaceOsc.stop(audioContext.value.currentTime + 0.08)
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