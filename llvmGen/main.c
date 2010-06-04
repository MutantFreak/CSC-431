#include <stdio.h>
#include <stdlib.h>

//C helpers
extern void print_val(long long a);
extern void halt_with_error(long long a, long long b);
extern void halt_with_error_noval(long long a);
extern void halt_with_error_firstword(long long a, long long b);
extern void halt_with_error_int(long long a, long long b);
//extern unsigned long long find_slot(%slots*,long long b);
//extern unsigned long long try_to_set_slot(%slots*, long long b, long long c);
extern void ding();
extern void print_int(long long a);

//C primitives:
extern long long add_prim(long long a, long long b);
extern long long equal_prim(long long a, long long b);
extern long long or_prim(long long a, long long b);
extern long long not_prim(long long a);
extern long long print_prim(long long a);
extern long long flexiplus_prim(long long a, long long b);
extern long long fleximinus_prim(long long a, long long b);
extern long long flexitimes_prim(long long a, long long b);
extern long long flexidivide_prim(long long a, long long b);
extern long long flexilessthan_prim(long long a, long long b);
extern long long flexigreaterthan_prim(long long a, long long b);
extern long long flexilessequalthan_prim(long long a, long long b);
extern long long flexigreaterequalthan_prim(long long a, long long b);
extern long long stringLength_prim(long long a);
extern long long subString_prim(long long a, long long b, long long c);
extern long long stringAppend_prim(long long a, long long b);
extern long long stringEqualHuh_prim(long long a, long long b);
extern long long stringLessThanHuh_prim(long long a, long long b);
extern long long stringHuh_prim(long long a);
extern long long floatHuh_prim(long long a);
extern long long plainHuh_prim(long long a);
extern long long closureHuh_prim(long long a);
extern long long instanceof_prim(long long a, long long b);
extern long long sqrt_prim(long long a);

//private helper
extern bool isInt(long long a);
extern bool isDouble(long long a);
extern bool isBool(long long a);
extern bool isVoid(long long a);
extern bool isString(long long a);

extern int getInt(long long a);
extern double getDouble(long long a);
extern bool getBool(long long a);
extern char* getString(long long a);

extern void print_val(long long a){

}
extern void halt_with_error(long long a, long long b){

}
extern void halt_with_error_noval(long long a){

}
extern void halt_with_error_firstword(long long a, long long b){

}
extern void halt_with_error_int(long long a, long long b){

}
//extern unsigned long long find_slot(%slots*,long long b);
//extern unsigned long long try_to_set_slot(%slots*, long long b, long long c);
extern void ding(){

}
extern void print_int(long long a){

}

extern long long add_prim(long long a, long long b){
  return a;
}
extern long long equal_prim(long long a, long long b){
  return a;
}
extern long long and_prim(long long a, long long b){
  return a & b;
}
extern long long or_prim(long long a, long long b){
  return a | b;
}
extern long long not_prim(long long a){
  //check to make sure it is bool
  if(isBool(a)){
    //get's the actual value
    if(getBool(a)){
      return 6;
    }
    else{
      return 14;
    }
  }
  fprintf(stderr,"not_prim error, not a boolean\n");
  exit(1);
}
extern long long print_prim(long long a){
  printf("a=%d",a);
  return a;
}
extern long long flexiplus_prim(long long a, long long b){
  return a;
}
extern long long fleximinus_prim(long long a, long long b){
  return a;
}
extern long long flexitimes_prim(long long a, long long b){
  return a;
}
extern long long flexidivide_prim(long long a, long long b){
  return a;
}
extern long long flexilessthan_prim(long long a, long long b){
  return a;
}
extern long long flexigreaterthan_prim(long long a, long long b){
  return a;
}
extern long long flexilessequalthan_prim(long long a, long long b){
  return a;
}
extern long long flexigreaterequalthan_prim(long long a, long long b){
  return a;
}
extern long long stringLength_prim(long long a){
  return a;
}
extern long long subString_prim(long long a, long long b, long long c){
  return a;
}
extern long long stringAppend_prim(long long a, long long b){
  return a;
}
extern long long stringEqualHuh_prim(long long a, long long b){
  return a;
}
extern long long stringLessThanHuh_prim(long long a, long long b){
  return a;
}
extern long long stringHuh_prim(long long a){
  return a;
}
extern long long floatHuh_prim(long long a){
  return a;
}
extern long long plainHuh_prim(long long a){
  return a;
}
extern long long closureHuh_prim(long long a){
  return a;
}
extern long long instanceof_prim(long long a, long long b){
  return a;
}
extern long long sqrt_prim(long long a){
  return a;
}


//--00 - int
//--01 - double
//--10 - bool or void
//-110 - bool
//0110 - false bool
//1110 - true bool
//-010 - void
//--11 - pointer
//0111 - string (Clements)
//1011 - closure (our decision)


extern bool isInt(long long a){
  long long aCheck = 3 & a;
  if(aCheck == 0){
    return true;
  }
  return false;
}
extern bool isDouble(long long a){
  long long aCheck = 3 & a;
  if(aCheck == 1){
    return true;
  }
  return false;
}
extern bool isBool(long long a){
  long long aCheck = 7 & a;
  if(aCheck == 6){
    return true;
  }
  return false;
}
extern bool isVoid(long long a){
  long long aCheck = 7 & a;
  if(aCheck == 2){
    return true;
  }
  return false;
}
extern bool isString(long long a){
  long long aCheck = 15 & a;
  if(aCheck == 7){
    return true;
  }
  return false;
}
extern bool isClosure(long long a){
  long long aCheck = 15 & a;
  if(aCheck == 11){
    return true;
  }
  return false;
}

extern int getInt(long long a){
  if(!isInt(a)){
    fprintf(stderr,"in getInt, this is not int!\n");
    exit(1);
  }
  return a/4;
}
extern double getDouble(long long a){
  if(!isDouble(a)){
    fprintf(stderr,"in getDouble, this is not double!\n");
    exit(1);
  }
  return a/4;
}
extern bool getBool(long long a){
  if(!isBool(a)){
    fprintf(stderr,"in getBool, this is not bool!\n");
    exit(1);
  }
  long long aCheck = 15 & a;
  if(aCheck == 6){
    return false;
  }
  else{
    return true;
  }
}
extern char* getString(long long a){
  if(!isString(a)){
    fprintf(stderr,"in getString, this is not string!\n");
    exit(1);
  }
  return NULL;//(char*)a;
}

int main(){
  //main_0(NULL);
  
}



