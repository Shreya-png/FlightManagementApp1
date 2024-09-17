export class Passenger {
    userId: string;
    name: string;
    age: number;
    gender: string;
    email: string;
    phone: string;
  
    constructor(
      userId: string = '',
      name: string = '',
      age: number = 0,
      gender: string = '',
      email: string = '',
      phone: string = ''
    ) {
      this.userId = userId;
      this.name = name;
      this.age = age;
      this.gender = gender;
      this.email = email;
      this.phone = phone;
    }
  }
  