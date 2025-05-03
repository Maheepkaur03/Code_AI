import ttkbootstrap as tb
from ttkbootstrap.constants import *
from tkinter import messagebox
from translate import Translator
from PIL import Image, ImageTk

# === LANGUAGE MAP ===
LANGUAGE_MAP = {
    "English": "en", "French": "fr", "Spanish": "es", "German": "de",
    "Italian": "it", "Portuguese": "pt", "Russian": "ru", "Arabic": "ar",
    "Chinese": "zh", "Japanese": "ja", "Korean": "ko", "Hindi": "hi"
}

# === TRANSLATION FUNCTION ===
def translate_text():
    source_text = source_textbox.get("1.0", "end-1c")
    source_lang_name = source_lang_combobox.get()
    target_lang_name = target_lang_combobox.get()

    if not source_text.strip():
        messagebox.showerror("Error", "Please enter text to translate.")
        return

    if source_lang_name not in LANGUAGE_MAP or target_lang_name not in LANGUAGE_MAP:
        messagebox.showerror("Error", "Please select valid languages.")
        return

    try:
        translator = Translator(
            from_lang=LANGUAGE_MAP[source_lang_name],
            to_lang=LANGUAGE_MAP[target_lang_name]
        )
        translated = translator.translate(source_text)

        target_textbox.delete("1.0", "end")
        target_textbox.insert("end", translated)

    except Exception as e:
        messagebox.showerror("Error", f"Translation failed: {e}")

# === MAIN WINDOW ===
app = tb.Window(themename="minty")
app.title("🌍 Project Language Translator")
app.geometry("1000x650")
app.state('zoomed')

# === LOAD AND SET BACKGROUND IMAGE ===
try:
    bg_image = Image.open(r"C:\Users\Maheep Kaur\Desktop\picture.jpg")  # Change path as needed
except Exception as e:
    messagebox.showerror("Image Error", f"Could not load background image.\n{e}")
    bg_image = Image.new("RGB", (1920, 1080), "white")

bg_image = bg_image.resize((app.winfo_screenwidth(), app.winfo_screenheight()), Image.Resampling.LANCZOS)
bg_photo = ImageTk.PhotoImage(bg_image)

# === CANVAS BACKGROUND ===
canvas = tb.Canvas(app, width=1920, height=1080)
canvas.pack(fill="both", expand=True)
canvas.create_image(0, 0, anchor="nw", image=bg_photo)
canvas.create_rectangle(0, 0, 1920, 1080, fill="black", stipple="gray50")  # semi-transparent overlay

# === MAIN CONTAINER FRAME ON TOP OF CANVAS ===
main_frame = tb.Frame(canvas, style="light.TFrame")
main_frame.place(relx=0.5, rely=0.05, relwidth=0.9, relheight=0.9, anchor="n")

# === HEADING ===
heading = tb.Label(
    main_frame,
    text="🌐 LANGUAGE TRANSLATOR",
    font=("Segoe UI", 28, "bold"),
    bootstyle="primary"
)
heading.pack(pady=(10, 20))

# === LANGUAGE SELECTORS ===
language_names = list(LANGUAGE_MAP.keys())
row1 = tb.Frame(main_frame)
row1.pack(pady=(10, 15))

tb.Label(row1, text="From:", font=("Segoe UI", 12)).pack(side="left", padx=10)
source_lang_combobox = tb.Combobox(row1, values=language_names, font=("Segoe UI", 11), width=18)
source_lang_combobox.set("English")
source_lang_combobox.pack(side="left", padx=10)

tb.Label(row1, text="To:", font=("Segoe UI", 12)).pack(side="left", padx=(30, 10))
target_lang_combobox = tb.Combobox(row1, values=language_names, font=("Segoe UI", 11), width=18)
target_lang_combobox.set("French")
target_lang_combobox.pack(side="left", padx=10)

# === TEXTBOX FRAME ===
textbox_frame = tb.Frame(main_frame)
textbox_frame.pack(fill="both", expand=True, pady=10)

# === SOURCE TEXT ===
tb.Label(textbox_frame, text="Enter Text:", font=("Segoe UI", 12, "bold")).grid(row=0, column=0, sticky="w", padx=10)
source_textbox = tb.Text(textbox_frame, height=12, width=50, font=("Segoe UI", 11), wrap="word")
source_textbox.grid(row=1, column=0, padx=10, pady=10)

# === TRANSLATED TEXT ===
tb.Label(textbox_frame, text="Translated Text:", font=("Segoe UI", 12, "bold")).grid(row=0, column=1, sticky="w", padx=10)
target_textbox = tb.Text(textbox_frame, height=12, width=50, font=("Segoe UI", 11), wrap="word")
target_textbox.grid(row=1, column=1, padx=10, pady=10)

# === TRANSLATE BUTTON ===
translate_button = tb.Button(
    main_frame,
    text="🚀 Translate Now",
    bootstyle="success-outline",
    width=25,
    command=translate_text
)
translate_button.pack(pady=(10, 20))

# === START MAIN LOOP ===
app.mainloop()
