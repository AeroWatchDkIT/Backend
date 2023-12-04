from flask import Flask, render_template, Response
import cv2
import pytesseract
import re
from datetime import datetime

# Create a Flask application instance
app = Flask(__name__)

# Setting the path to the Tesseract executable
pytesseract.pytesseract.tesseract_cmd = '/usr/bin/tesseract'

# Function to preprocess the frame for OCR
def preprocess_image_for_ocr(frame):
    # Convert the frame to grayscale
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    
    # Apply thresholding - improves contrast by making pixels either black or white
    _, thresh = cv2.threshold(gray, 150, 255, cv2.THRESH_BINARY)

    # Resize the image - improving OCR accuracy
    resized = cv2.resize(thresh, None, fx=1.5, fy=1.5, interpolation=cv2.INTER_LINEAR)

    return resized

# Function to detect text in the frame
def detect_features(frame):
    preprocessed_frame = preprocess_image_for_ocr(frame)
    
    # Configuring pytesseract to use a specific OCR Engine Mode (OEM) and Page Segmentation Mode (PSM)
    custom_config = r'--oem 3 --psm 6'
    
    # Use pytesseract to extract text data from the image get the bounding box coordinates and other data of each text segment
    data = pytesseract.image_to_data(preprocessed_frame, config=custom_config, output_type=pytesseract.Output.DICT)

    full_text = ""  # String to accumulate detected text

    for i in range(len(data['text'])):
        # Process each text segment if the confidence level is high and text is non-empty
        if int(data['conf'][i]) > 60 and data['text'][i].strip() != '':
            x, y, w, h = data['left'][i], data['top'][i], data['width'][i], data['height'][i]
            
           # Accumulate recognized text
            full_text += data['text'][i] + " "
            
            # Draw a green rectangle around each recognized text segment
            frame = cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

    # # Regular expression to match a specific pattern in the recognized text for example 'A1.C1.R1'
    pattern = r'\b[A-Z]\d\.[C][1-9]\.[R][1-9]\b'
    matches = re.findall(pattern, full_text)

    # Print the matched text with the current time if the pattern is found
    if matches:
        current_time = datetime.now().strftime("%H:%M:%S")  # Get the current time
        print(f"Detected Text: {' '.join(matches)} at {current_time}")  # Print text with time

    return frame

# Function to generate frames for video streaming
def gen_frames():  
    stream = cv2.VideoCapture(0)
    stream.set(cv2.CAP_PROP_FRAME_WIDTH, 640) # Set frame width
    stream.set(cv2.CAP_PROP_FRAME_HEIGHT, 480) # Set frame height

    while True:
        success, frame = stream.read() # Read a frame from the video stream
        if not success:
            break
        else:
            frame = detect_features(frame)  # Apply OCR to each frame
            ret, buffer = cv2.imencode('.jpg', frame) # Encode the frame as JPEG
            frame = buffer.tobytes() # Convert the frame into bytes
            yield (b'--frame\r\n'
                   b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n')

# Route to the home page
@app.route('/')
def index():
    # Render template (make sure you have an 'index.html' template in 'templates' directory)
    return render_template('index.html')


# Route to the video stream in frontend cabin view
@app.route('/video_feed')
def video_feed():
    # Video streaming route. Put this in the src attribute of an img tag in 'index.html'
    return Response(gen_frames(), mimetype='multipart/x-mixed-replace; boundary=frame')

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=True) # Run the Flask application

