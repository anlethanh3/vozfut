import axios from "axios"
import { ProfileProps, TokenProps } from "../slices/profileSlice"
import { LoginRequestProps } from "../slices/profileSlice"

const url = `${process.env.REACT_APP_API_URL}/user`

export const authenticate = async (props: { signal: AbortSignal, data: LoginRequestProps }) => {
    let { signal, data } = props
    let response = await axios.post<TokenProps>(`${url}/authenticate`, data, { signal: signal })
    return response
}

export const profile = async (props: { signal: AbortSignal, token: TokenProps }) => {
    const { token, signal } = props
    const { accessToken, tokenType } = token
    let response = await axios.get<ProfileProps>(`${url}/profile`, { signal: signal, headers: { "Authorization": `${tokenType} ${accessToken}` } })
    return response
}
