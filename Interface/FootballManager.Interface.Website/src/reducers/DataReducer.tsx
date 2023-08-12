import { Reducer } from 'react'

export interface ProfileProps {
    userId: number,
    email: string,
    username: string,
    role: string,
}
export interface TokenProps {
    tokenType: string,
    expiredIn: number,
    accessToken: string,
}

export interface State {
    token: TokenProps | undefined,
    profile: ProfileProps | undefined,
}

export const initState: State = {
    token: undefined,
    profile: undefined,
}

export type Action =
    | { type: 'profile', profile: ProfileProps }
    | { type: 'token', token: TokenProps }
    | { type: 'signout' }

export const reducer: Reducer<State, Action> = (state, action) => {
    switch (action.type) {
        case 'profile':
            return { ...state, profile: action.profile }
        case 'token':
            return { ...state, token: action.token }
        case 'signout':
            return { ...state, token: undefined, profile: undefined }
        default:
            return state;
    }
}