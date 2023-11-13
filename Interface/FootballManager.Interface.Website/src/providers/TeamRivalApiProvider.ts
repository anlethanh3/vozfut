import axios from "axios"
import { ExchangeMemberProps, RollingProps, UpdateRivalMemberProps } from "../slices/matchDetailSlice"

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

export const exchangeMembers = async <T>(props: { signal: AbortSignal, data: ExchangeMemberProps }) => {
    let { data, signal } = props
    let response = await axios.post<T>(`${url}/${data.matchId}/exchange`, data, { signal: signal })
    return response
}

export const updateRivalMember = async <T>(props: { signal: AbortSignal, data: UpdateRivalMemberProps }) => {
    let { data, signal } = props
    let response = await axios.post<T>(`${url}/${data.matchId}/memberInOut`, data, { signal: signal })
    return response
}

export const updateSquad = async <T>(props: { signal: AbortSignal, matchId: number, data: RollingProps[] }) => {
    let { data, signal, matchId } = props
    let response = await axios.post<T>(`${url}/${matchId}/squad`, data, { signal: signal })
    return response
}