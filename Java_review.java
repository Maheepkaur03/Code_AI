import java.util.Scanner; // Used, but fine
import java.util.Random;  // ❌ Unused import
import java.util.ArrayList; // ❌ Unused import

class calculatorapp {  // ❌ Class name should be PascalCase

  int res = 0;  // ❌ Class-level mutable variable, bad practice for stateless operation

  public static void main(String args[]) {
    Scanner input = new Scanner(System.in);
    calculatorapp obj = new calculatorapp();

    System.out.println("Enter first number:");
    int a = input.nextInt();

    System.out.println("Enter second number:");
    int b = input.nextInt();

    System.out.println("Choose operation: 1-Add 2-Subtract 3-Multiply 4-Divide");
    int choice = input.nextInt();

    if (choice == 1) {
      obj.res = a + b;
      System.out.println("Result: " + obj.res);
    } else if (choice == 2) {
      obj.res = a - b;
      System.out.println("Result: " + obj.res);
    } else if (choice == 3) {
      obj.res = a * b;
      System.out.println("Result: " + obj.res);
    } else if (choice == 4) {
      if (b != 0) {
        obj.res = a / b;
        System.out.println("Result: " + obj.res);
      } else {
        System.out.println("Division by zero not allowed");
      }
    } else if (choice == 1) {  // ❌ Duplicate condition — unreachable
      System.out.println("You chose addition again?");
    } else {
      System.out.println("Invalid choice");
    }

    input.close();
  }

  void unusedMethod() {
    int temp = 100; // ❌ Unused variable
    // No implementation
  }
}
