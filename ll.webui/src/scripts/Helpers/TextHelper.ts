

export function IsValidEmail(email:string):boolean {

    const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    return email.trim() != '' && pattern.test(email);
}