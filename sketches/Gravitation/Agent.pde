class Agent{
  color colour;
  
  PVector position = new PVector();
  PVector velocity = new PVector();
  PVector force = new PVector();
  
  float size = 10;
  float mass;
  
  boolean doneCalcingGravity = false;
  
  Agent(float massMin, float massMax){
    position.x = random(0, width);
    position.y = random(0, height);
    
    mass = random(massMin, massMax);
    size = sqrt(mass);
    
    colorMode(HSB);
    colour = color(random(0, 255), 255, 255);
  }
  
  void update(){
    for(Agent a : agents){
      if (a == this) continue; //Skip
      if (a.doneCalcingGravity) continue; //Skip
      
      PVector f = findGravityForce(a);
      force.add(f);
    }
    doneCalcingGravity = true;
    
    //A = F/M
    PVector acceleration = PVector.div(force, mass);
    
    //Clear force
    force.set(0, 0);
    
    //V += A
    velocity.add(acceleration);
    
    //P += V
    position.add(velocity);
  }
  
  void draw(){
    fill(colour);
    ellipse(position.x, position.y, size, size);
  }
  
  PVector findGravityForce(Agent a){
    PVector vToAgentA = PVector.sub(a.position, position);
    
    float r = vToAgentA.mag();
    
    float gravForce = G * (a.mass * mass) / (r * r);
    
    if (gravForce > maxForce) gravForce = maxForce;
    
    vToAgentA.normalize();
    vToAgentA.mult(gravForce);
    
    a.force.add(PVector.mult(vToAgentA, -1));
    
    return vToAgentA;
  }
}
