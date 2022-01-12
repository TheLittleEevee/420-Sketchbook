//float[] valsWave;
//float[] valsNoise;
//float[] valsRand;

//FloatList vals;

ArrayList<PVector> vals = new ArrayList<PVector>();

void setup(){
  size(500, 600, P2D);
  stroke(255);
  strokeWeight(2);
}

void draw(){
  background(0);
  
  //Add new values to our array
  float time = millis() / 1000.0;
  
  float valWave = map(sin(time), -1, 1, 0, 1); //Sine gives values between -1 and 1
  float valRand = random(0, 1);
  float valNoise = noise(time); //Noise gives values between 0 and 1
  
  vals.add(0, new PVector(valWave, valRand, valNoise)); //(Where to insert, What to insert
  
  //Remove last item if too many
  if(vals.size() > width) vals.remove(vals.size() - 1);
  
  //Draw three arrays to screen
  float third = height / 3;
  
  for(int x = 0; x < vals.size(); x++){
    PVector set = vals.get(x);
    float v1 = set.x;
    float v2 = set.y;
    float v3 = set.z;
    
    float y1 = third - v1 * third;
    float y2 = third * 2 - v2 * third;
    float y3 = height - v3 * third;

    point(x, y1);
    point(x, y2);
    point(x, y3);
  }
}
