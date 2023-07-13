export class DriverInfoDTO {
   Id: number;
   Name: string;
   PhotoLink?: string;
   Experience: number;
   AverageRating: number;

   constructor() {
     this.Id = 0;
     this.Name = '';
     this.Experience = 0;
     this.AverageRating = 0;
   }
}
