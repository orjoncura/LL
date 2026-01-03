import { POST, GET } from '@/utils/Security/httpClient'
import { LoginModel, TokenViewModel, ConfirmationModel, NewUserModel, ProfileViewModel } from '@/utils/Models/models';

export async function Authenticate(email:string, password:string): Promise<TokenViewModel> {

    const data: LoginModel = {
        "email": email,
        "password": password
    };

    return await POST<TokenViewModel>('/Security/Authenticate', JSON.stringify(data));
}

export async function RegisterUser(email:string, confirmEmail:string): Promise<boolean> {

    const data: NewUserModel = { "email": email, "confirmEmail": confirmEmail};

    return await POST<boolean>('/Security/RegisterUser', JSON.stringify(data));
}

export async function CompleteUserRegistration(data: ConfirmationModel): Promise<boolean> {
    return await POST<boolean>('/Security/CompleteUserRegistration', JSON.stringify(data));
}

export async function ResetPassword(email:string): Promise<boolean> {
    return await POST<boolean>('/Security/ResetPassword', JSON.stringify(email));
}

export async function CompletePasswordReset(data:ConfirmationModel): Promise<boolean> {
    return await POST<boolean>('/Security/CompletePasswordReset', JSON.stringify(data));
}

export async function GetProfileDetails(): Promise<ProfileViewModel> {
  return await GET<ProfileViewModel>('/Security/GetProfileDetails');
}

export async function LogOut(): Promise<boolean> {
    return await POST<boolean>('/Security/Logout', "");
}