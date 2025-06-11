import { Login } from "./login";
import { Register } from "./register";

export function createDefaultLogin(): Login {
    return {
        username: '',
        password: ''
    };
}

export function createDefaultRegister(): Register {
    return {
        username: '',
        password: ''
    };
}