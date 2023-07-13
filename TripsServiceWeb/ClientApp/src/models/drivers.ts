export class DriverInfoDTO {
   id: number;
   name: string;
   photoLink?: string;
   experience: number;
   averageRating: number;

   constructor() {
     this.id = 0;
     this.name = '';
     this.experience = 0;
     this.averageRating = 0;
   }
}
