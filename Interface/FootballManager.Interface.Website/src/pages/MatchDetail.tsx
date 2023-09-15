import { useEffect } from "react";
import { Button, Col, Row, Table, Alert, Form, OverlayTrigger, Tooltip } from "react-bootstrap";
import moment from 'moment';
import { selectState, fetchAsync, rollingAsync, onCloseError, onCloseRivals, fetchMembersAsync, onShowAdd, MatchDetailProps, addMatchDetailAsync, deleteMatchDetailAsync, fetchMatchAsync, updateMatchDetailAsync, RollingProps, onShowExchange, exchangeMembersAsync, ExchangeMemberProps } from '../slices/matchDetailSlice';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { useParams } from "react-router-dom";
import Rivals from "../components/Rivals";
import AddMatchDetail from "../components/AddMatchDetail";
import { FaExchangeAlt, FaFutbol } from "react-icons/fa";
import ExchangeMember from "../components/ExchangeMember";
import { MemberProps } from "../slices/memberSlice";

export default function MatchDetail() {
    let { id } = useParams()
    var matchId = parseInt(id ?? "")
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ id: matchId })).unwrap()
            .then(value => dispatch(fetchMembersAsync(matchId)).unwrap())
            .catch(ex => { console.log(ex) })
    }

    const rolling = () => {
        dispatch(rollingAsync({ id: parseInt(id ?? "") }))
            .unwrap()
            .then(values => {
            })
            .catch(error => {
            })
    }

    const onAddSubmit = (detail: MatchDetailProps) => {
        dispatch(onShowAdd(false))
        dispatch(addMatchDetailAsync(detail)).unwrap()
            .then(value => fetch())
            .catch(err => { })
    }

    const onDelete = (id: number) => {
        dispatch(deleteMatchDetailAsync(id)).unwrap()
            .then(value => fetch())
            .catch(err => { })
    }

    useEffect(() => {
        console.log('effect', state)
        dispatch(fetchMatchAsync({ id: matchId })).unwrap()
            .then(value => fetch())
            .catch(ex => { console.log(ex) })

        return function cleanup() {
            console.log('unmount', state)
        }
    }, [])

    function memberReadyCount(): number {
        var count = 0
        state.data.forEach(item => {
            if (item.isPaid === true && item.isSkip === false) {
                count += 1
            }
        })
        return count
    }

    function isInvalidRivals() {
        // if (state.match === undefined) {
        //     return true
        // }
        // var count = memberReadyCount()
        // if (count < state.match.teamCount * state.match.teamSize) {
        //     return true
        // }
        return false
    }
    function onSubmitRivals(rivals: RollingProps[]) {
        dispatch(onCloseRivals())
    }

    const updateDetail = (data: MatchDetailProps) => {
        dispatch(updateMatchDetailAsync({ data: data })).unwrap()
            .then(value => fetch())
            .catch(ex => { console.log(ex) })
    }

    const exchangeMembers = (data: ExchangeMemberProps) => {
        dispatch(exchangeMembersAsync({ data: { ...data, matchId: matchId } })).unwrap()
            .catch(ex => { console.log(ex) })
    }

    return (
        <>
            {
                state.match &&
                <>
                    <h1>{state.match.name}</h1>
                    <h2>{state.match.description}</h2>
                </>
            }
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.isShowAdd &&
                <AddMatchDetail show={state.isShowAdd} matchId={parseInt(id ?? "")}
                    members={state.members} onSubmit={(detail) => { onAddSubmit(detail) }} onClose={() => dispatch(onShowAdd(false))} />
            }
            {
                state.isShowRivals && state.rolling && state.match &&
                <Rivals onSubmit={(rivals) => onSubmitRivals(rivals)} show={state.isShowRivals} rivals={state.rolling} match={state.match} onClose={() => dispatch(onCloseRivals())} />
            }
            {
                state.isShowExchange && state.members &&
                <ExchangeMember onSubmit={(data) => exchangeMembers(data)} show={state.isShowExchange} members={state.members} onClose={() => dispatch(onShowExchange(false))} />
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch(onCloseError())} dismissible>
                    {state.error}
                </Alert>
            }
            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <OverlayTrigger overlay={<Tooltip>Exchange Members</Tooltip>}>
                        <Button className="me-2" disabled={isInvalidRivals()} variant="secondary" onClick={() => { dispatch(onShowExchange(true)) }}><FaExchangeAlt /></Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>Team Division Rivals</Tooltip>}>
                        <Button disabled={isInvalidRivals()} variant="success" onClick={() => { rolling() }}><FaFutbol /></Button>
                    </OverlayTrigger>
                </Col>
            </Row>
            {
                <h3>Member paid count: {memberReadyCount()}</h3>
            }
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>No</th>
                        <th>Member</th>
                        <th>Is paid</th>
                        <th>Is Skip</th>
                        <th>Modified Date</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map((value, index) =>
                            <tr key={`key-${index}`}>
                                <td>{index + 1}</td>
                                <td>{value.memberName}</td>
                                <td><Form.Check onChange={(e) => { updateDetail({ ...state.data[index], isPaid: e.target.checked }) }} checked={value.isPaid} type="switch" /></td>
                                <td><Form.Check onChange={(e) => { updateDetail({ ...state.data[index], isSkip: e.target.checked }) }} checked={value.isSkip} type="switch" /></td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
        </>
    )
}