export interface Task {
  id: number;
  name: string;
  status: number;
  description: string;
  color: string;
  assignedUserId: number;
  assignedUserName: string;
}
