import whisper
import sys
import subprocess
import os

url = sys.argv[1] if len(sys.argv) > 1 else None
if not url:
    raise ValueError("You must provide a YouTube URL.")

subprocess.run([
    "yt-dlp", "-f", "bestaudio",
    "-o", "audio.%(ext)s",
    url
], check=True)

audio_file = next(
    f for f in os.listdir(".")
    if f.startswith("audio.") and not f.endswith(".wav")
)

subprocess.run([
    "ffmpeg", "-y", "-i", audio_file,
    "-ar", "16000", "-ac", "1",
    "-c:a", "pcm_s16le",
    "audio.wav"
], check=True)

model = whisper.load_model("base")
result = model.transcribe("audio.wav")

with open("transcript.txt", "w", encoding="utf-8") as f:
    f.write(result["text"])

print(result["text"])

