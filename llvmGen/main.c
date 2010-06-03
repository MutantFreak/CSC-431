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
  long long aCheck = 3 | a;
  long long bCheck = 3 | b;
  if(aCheck != 0 || bCheck != 0){
    fprintf(stderr,"add_prim error\n");
    exit(1);
  }
  return (a/4)+(b/4);
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
  return a;
}
extern long long print_prim(long long a){
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

int main(){
  main_0(NULL);
}



