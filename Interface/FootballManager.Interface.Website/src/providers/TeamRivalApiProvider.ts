import axios from "axios"
import { ExchangeMemberProps } from "../slices/matchDetailSlice"

const url = `${process.env.REACT_APP_API_URL}/teamrival`

export const get = async <T>(props: { signal: AbortSignal, id: number }) => {
    let { id, signal } = props
    let response = await axios.get<T>(`${url}/${id}`, { signal: signal })
    return response
}

export const getAnonymous = async <T>(props: { signal: AbortSignal, id: number }) => {
    let { id, signal } = props
    let response = await axios.get<T>(`${url}/${id}/anonymous`, { signal: signal })
    return response
}

export const exchangeMembers = async <T>(props: { signal: AbortSignal, data:ExchangeMemberProps }) => {
    let { data, signal } = props
    let response = await axios.post<T>(`${url}/${data.matchId}/exchange`,data, { signal: signal })
    return response
}